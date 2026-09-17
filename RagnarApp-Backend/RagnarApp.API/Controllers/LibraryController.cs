using AutoMapper;
using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RagnarApp.Application.Abstract;
using RagnarApp.Application.DTOs.LibraryDTOs;
using RagnarApp.Application.Queries.LibraryQueries;
using System.Formats.Asn1;
using System.Globalization;

namespace RagnarApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _publisher;

        public LibraryController(IMediator mediator, ILogger<LibraryController> logger, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
            _publisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            _logger.LogInformation($"Get libraries ordered by import date descending");
            var query = new GetLibrariesQuery();
            var result = await _mediator.Send(query);
            var toReturn = _mapper.Map<List<LibraryToReturn>>(result);

            return Ok(toReturn);
        }

        [HttpPost("imports")]
        public async Task<IActionResult> ImportBooks(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Create new books");
            if (file is null)
            {
                _logger.LogWarning($"File is required.");
                return BadRequest("File is required.");
            }

            if (file.Length == 0)
            {
                return BadRequest("CSV file is empty.");
            }

            if (!Path.GetExtension(file.FileName)
                .Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only CSV files are supported.");
            }

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            using var csv = new CsvReader(
                reader,
                CultureInfo.InvariantCulture);

            var books = csv
                .GetRecords<ImportLibraryDTO>()
                .ToList();

            if (!books.Any())
            {
                _logger.LogWarning($"CSV contains no records.");
                return BadRequest("CSV contains no records.");
            }

            foreach (var book in books)
            {
                await _publisher.PublishAsync(
                    book,
                    cancellationToken);
            }

            return Accepted(new
            {
                QueuedBooks = books.Count
            });
        }
    }
}
