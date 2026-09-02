using System;
using System.Threading;
using Frends.Edifact.ConvertToJson.Definitions;
using Frends.Edifact.ConvertToJson.Helpers;
using NUnit.Framework;

namespace Frends.Edifact.ConvertToJson.Tests;

[TestFixture]
internal class ErrorHandlerTest
{
    private const string CustomErrorMessage = "CustomErrorMessage";

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var options = TestHelpers.DefaultOptions();
        options.ThrowErrorOnFailure = true;

        var ex = Assert.Throws<AggregateException>(() =>
            Edifact.ConvertToJson(
                new Input { InputEdifact = "INVALID_EDIFACT_DATA", AllowMissingUNB = true },
                options,
                CancellationToken.None));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public void Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = TestHelpers.DefaultOptions();
        options.ThrowErrorOnFailure = false;

        var result = Edifact.ConvertToJson(
            new Input { InputEdifact = "INVALID_EDIFACT_DATA", AllowMissingUNB = true },
            options,
            CancellationToken.None);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Message, Is.Not.Empty);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = TestHelpers.DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;

        var ex = Assert.Throws<Exception>(() =>
            Edifact.ConvertToJson(
                new Input { InputEdifact = "INVALID_EDIFACT_DATA", AllowMissingUNB = true },
                options,
                CancellationToken.None));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Contains.Substring(CustomErrorMessage));
    }

    [Test]
    public void ThrowIfCanceled_Should_Throw_OperationCanceledException_When_ThrowCanceled_Is_True()
    {
        var options = TestHelpers.DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var canceledException = new OperationCanceledException("Operation was canceled");

        var ex = Assert.Throws<OperationCanceledException>(() =>
            canceledException.Handle(options, throwCanceled: true));
        Assert.That(ex, Is.SameAs(canceledException));
        Assert.That(ex?.Message, Is.EqualTo("Operation was canceled"));
    }

    [Test]
    public void Should_Format_ErrorMessage_In_Result_When_CustomMessage_Is_Provided()
    {
        var options = TestHelpers.DefaultOptions();
        options.ThrowErrorOnFailure = false;
        options.ErrorMessageOnFailure = CustomErrorMessage;
        var exception = new Exception("Original error message");

        var result = exception.Handle(options);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Is.EqualTo($"{CustomErrorMessage}: Original error message"));
    }

    [Test]
    public void ThrowIfCanceled_Should_Not_Throw_When_ThrowCanceled_Is_False()
    {
        var options = TestHelpers.DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var canceledException = new OperationCanceledException("Operation was canceled");

        var result = canceledException.Handle(options, throwCanceled: false);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.SameAs(canceledException));
    }
}
