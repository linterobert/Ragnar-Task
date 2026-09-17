using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RagnarApp.API.Controllers;
using RagnarApp.Application.Abstract;

namespace RagnarApp.UnitTest;

public class LibraryControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<LibraryController>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMessagePublisher> _publisherMock;

    private readonly LibraryController _controller;

    public LibraryControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<LibraryController>>();
        _mapperMock = new Mock<IMapper>();
        _publisherMock = new Mock<IMessagePublisher>();

        _controller = new LibraryController(
            _mediatorMock.Object,
            _loggerMock.Object,
            _mapperMock.Object,
            _publisherMock.Object);
    }


    [Fact]
    public async Task ImportBooks_Should_Return_BadRequest_When_File_Is_Empty()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(x => x.Length).Returns(0);
        fileMock.Setup(x => x.FileName).Returns("books.csv");

        // Act
        var result = await _controller.ImportBooks(
            fileMock.Object,
            CancellationToken.None);

        // Assert
        var badRequest = result as BadRequestObjectResult;

        badRequest.Should().NotBeNull();
        badRequest!.Value.Should().Be("CSV file is empty.");
    }

    [Fact]
    public async Task ImportBooks_Should_Return_BadRequest_When_File_Is_Not_Csv()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(x => x.Length).Returns(100);
        fileMock.Setup(x => x.FileName).Returns("books.txt");

        // Act
        var result = await _controller.ImportBooks(
            fileMock.Object,
            CancellationToken.None);

        // Assert
        var badRequest = result as BadRequestObjectResult;

        badRequest.Should().NotBeNull();
        badRequest!.Value.Should().Be("Only CSV files are supported.");
    }
}