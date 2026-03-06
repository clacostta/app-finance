using FinanceApp.Infrastructure.Services;
using FluentAssertions;

namespace FinanceApp.Tests.Infrastructure;

public class OfxParserTests
{
    [Fact]
    public void Parse_ShouldExtractTransactionsFromValidOfx()
    {
        var ofx = """
<OFX>
  <BANKMSGSRSV1>
    <STMTTRNRS>
      <STMTRS>
        <BANKACCTFROM>
          <BANKID>260
          <ACCTID>000123
        </BANKACCTFROM>
        <BANKTRANLIST>
          <STMTTRN>
            <TRNTYPE>DEBIT
            <DTPOSTED>20250105120000
            <TRNAMT>-45.90
            <FITID>abc-1
            <MEMO>IFOOD PEDIDO 123
          </STMTTRN>
          <STMTTRN>
            <TRNTYPE>CREDIT
            <DTPOSTED>20250106120000
            <TRNAMT>5000.00
            <FITID>abc-2
            <MEMO>SALARIO
          </STMTTRN>
        </BANKTRANLIST>
      </STMTRS>
    </STMTTRNRS>
  </BANKMSGSRSV1>
</OFX>
""";

        var parser = new OfxParser();

        var result = parser.Parse(ofx);

        result.Transactions.Should().HaveCount(2);
        result.AccountId.Should().Be("000123");
        result.Transactions[0].Description.Should().Contain("IFOOD");
        result.Transactions[0].Amount.Should().Be(-45.90m);
    }
}
