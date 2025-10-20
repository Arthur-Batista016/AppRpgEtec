using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value,Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value!=null)
            {
                byte[] imageAsytes = (byte[])value;
                retSource = new ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return retSource;
        }

        public object ConvertBlack(object value, Type targetType,object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
