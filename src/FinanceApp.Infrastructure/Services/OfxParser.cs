using System.Globalization;
using System.Text.RegularExpressions;
using FinanceApp.Application.Abstractions;
using FinanceApp.Application.DTOs;

namespace FinanceApp.Infrastructure.Services;

public class OfxParser : IOfxParser
{
    public OfxParseResultDto Parse(string ofxContent)
    {
        if (string.IsNullOrWhiteSpace(ofxContent))
            throw new InvalidOperationException("Arquivo OFX vazio.");

        var result = new OfxParseResultDto
        {
            AccountId = GetTagValue(ofxContent, "ACCTID"),
            BankId = GetTagValue(ofxContent, "BANKID")
        };

        var stmts = Regex.Matches(ofxContent, "<STMTTRN>(.*?)</STMTTRN>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        if (stmts.Count == 0)
        {
            result.Warnings.Add("Nenhuma transação encontrada no arquivo OFX.");
            return result;
        }

        foreach (Match m in stmts)
        {
            try
            {
                var trn = m.Groups[1].Value;
                var type = GetTagValue(trn, "TRNTYPE") ?? "DEBIT";
                var amountRaw = GetTagValue(trn, "TRNAMT") ?? "0";
                var fitId = GetTagValue(trn, "FITID");
                var memo = GetTagValue(trn, "MEMO") ?? GetTagValue(trn, "NAME") ?? "Sem descrição";
                var dt = ParseOfxDate(GetTagValue(trn, "DTPOSTED") ?? GetTagValue(trn, "DTUSER"));

                if (!decimal.TryParse(amountRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                {
                    result.Warnings.Add($"Valor inválido para transação '{memo}'.");
                    continue;
                }

                result.Transactions.Add(new OfxTransactionDto(
                    fitId,
                    dt,
                    dt,
                    amount,
                    memo.Trim(),
                    type,
                    result.AccountId,
                    null));
            }
            catch (Exception ex)
            {
                result.Warnings.Add($"Falha ao processar transação OFX: {ex.Message}");
            }
        }

        return result;
    }

    private static string? GetTagValue(string source, string tag)
    {
        var match = Regex.Match(source, $"<{tag}>([^\r\n<]+)", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    private static DateTime ParseOfxDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DateTime.UtcNow;

        var clean = value.Trim();
        if (clean.Length >= 8)
        {
            var normalized = clean.Length >= 14 ? clean[..14] : clean[..8];

            if (DateTime.TryParseExact(normalized, new[] { "yyyyMMddHHmmss", "yyyyMMdd" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt))
            {
                return dt;
            }
        }

        return DateTime.UtcNow;
    }
}
