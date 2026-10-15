using GxShared.GxDtos;
using GxShared.Others;

namespace GxTie.Components.PfrmHelper
{
    public static class ImmatHelper
    {
        public static ImmatriculationDto MapFromConfig(List<GsgfixDto> config)
        {
            var dto = new ImmatriculationDto();

            foreach (var seg in config)
            {
                // If it's user input, leave empty so UI can bind later
                string value = seg.Ityp == 1 ? "" : seg.Ftl4;

                switch (seg.Ftl2)
                {
                    case "Xmatri":
                        dto.Xmatri = value;
                        break;
                    case "Smatri":
                        dto.Smatri = value;
                        break;
                    case "Padres":
                        dto.Padres = value;
                        break;
                    case "Address":
                        dto.Address = value;
                        break;

                    case "SCUST":
                        dto.CustomerSmatri = value;
                        break;
                    case "SSUP":
                        dto.SupplierSmatri = value;
                        break;
                    case "SPART":
                        dto.PartnerSmatri = value;
                        break;
                    case "SAFF":
                        dto.AffiliateSmatri = value;
                        break;
                }
            }

            return dto;
        }
    }
}