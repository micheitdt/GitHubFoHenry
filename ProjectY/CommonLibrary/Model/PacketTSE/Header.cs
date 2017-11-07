
namespace CommonLibrary.Model.PacketTSE
{
    public class Header
    {
        public int Length { get; set; }
        public int BusinessCode { get; set; }
        public int FormatCode { get; set; }
        public int Version { get; set; }
        public int Seq { get; set; }
        public Header(byte[] data)
        {
            Length = Functions.ConvertToFormat9(data, 1, 2);
            BusinessCode = Functions.ConvertToFormat9(data, 3, 1);
            FormatCode = Functions.ConvertToFormat9(data, 4, 1);
            Version = Functions.ConvertToFormat9(data, 5, 1);
            Seq = Functions.ConvertToFormat9(data, 6, 4);
        }
    }
}
