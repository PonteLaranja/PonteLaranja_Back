using API.Applications.Conversoes;
using API.DTOs.TransferenciaDto;
using API.Interfaces;
using API.Exceptions;
using API.Domains;

namespace API.Applications.Services
{
    public class TransferenciaService
    {
        private readonly ITransferenciaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEstadoTransferenciaRepository _estadoTransferenciaRepository;

        public TransferenciaService(ITransferenciaRepository repository, IUsuarioRepository usuarioRepository, IEstadoTransferenciaRepository estadoTransferenciaRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _estadoTransferenciaRepository = estadoTransferenciaRepository;
        }

        public List<LerTransferenciaDto> Listar()
        {
            return _repository.Listar().Select(t => TransferenciaConversao.ConverterParaDto(t)).ToList();
        }

        public LerTransferenciaDto ObterPorID(Guid id)
        {
            var transferencia = _repository.ObterPorId(id);
            if(transferencia == null)
            {
                throw new DomainException("Transferencia não encontrada");
            }

            return TransferenciaConversao.ConverterParaDto(transferencia);
        }

        public void Adicionar(Guid usuarioID, CriarTransferenciaDto transferencia)
        {
            var usuario = _usuarioRepository.ObterPorId(usuarioID);

            if(usuario == null)
            {
                throw new DomainException("Usuario não encontrado.");
            }

            if (!_repository.ItemExiste(transferencia.ItemID))
            {
                throw new DomainException("Item não existe.");
            }

            if (!_repository.UnidadeExiste(transferencia.UnidadeDestinoID))
            {
                throw new DomainException("Unidade não existe.");
            }

            if (!_repository.UnidadeExiste(transferencia.UnidadeOrigemID))
            {
                throw new DomainException("Unidade não existe.");
            }



            if (usuario.TipoUsuario.NomeTipoUsuario == "Doador")
            {
                throw new DomainException("Apenas autorizados podem solicitar transferencia.");
            }

            _repository.Cadastrar(TransferenciaConversao.ConverterParaDomain(transferencia));
        }

        public void Responder(Guid transferenciaID, Guid usuarioID, ResponderSolicitacaoDto dto)
        {
            var usuario = _usuarioRepository.ObterPorId(usuarioID);

            if (usuario == null)
            {
                throw new DomainException("Usuario não encontrado");
            }

            var transferencia = _repository.ObterPorId(transferenciaID);

            if (!_repository.ItemExiste(transferencia.ItemID))
            {
                throw new DomainException("Item não existe.");
            }

            if (!_repository.UnidadeExiste(transferencia.UnidadeDestinoID))
            {
                throw new DomainException("Unidade não existe.");
            }

            if (!_repository.UnidadeExiste(transferencia.UnidadeOrigemID))
            {
                throw new DomainException("Unidade não existe.");
            }

            EstadoTransferencia estado;

            if (dto.Aprovado)
            {
                estado = _estadoTransferenciaRepository.ObterPorNome("Aprovado");
            }
            else
            {
                estado = _estadoTransferenciaRepository.ObterPorNome("Recusado");
            }

            if (estado == null)
            {
                throw new DomainException("Status de resposta da transferência não encontrado.");
            }

            transferencia.EstadoTransferenciaID = estado.EstadoTransID;

            //Alterar depois para que só atualize quando o front tiver um botão para garantir a chegada
            //Caso não tenha deixe essa função
            transferencia.DataChegada = DateTime.Now;

            _repository.Atualizar(transferencia.TransferenciaID, transferencia);
        }
    }
}
