using System.Collections.Generic;
using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public class FalseInformationFinder
    {
        public async Task<IReadOnlyCollection<FalseInformation>> GetAll(string siteUrl)
        {
            await Task.Delay(0);

            return new List<FalseInformation> {
                new FalseInformation {
                    firstTextNodeXPath = "/html/body/p[1]/text()[1]",
                    lastTextNodeXPath = "/html/body/p[1]/text()[1]",
                    offsetStart = 55,
                    offsetEnd = 165,
                    text = ""
                }
            };
        }
    }
}