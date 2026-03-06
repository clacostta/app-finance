using FinanceApp.Application.DTOs;

namespace FinanceApp.Application.Abstractions;

public interface IOfxParser
{
    OfxParseResultDto Parse(string ofxContent);
}
