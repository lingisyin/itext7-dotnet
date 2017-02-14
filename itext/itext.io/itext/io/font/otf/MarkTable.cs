/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
namespace iText.IO.Font.Otf {
    /// <author>psoares</author>
    public class MarkTable {
        private MarkTable() {
        }

        public static bool IsMark(int charPoint) {
            int p = iText.IO.Util.JavaUtil.ArraysBinarySearch(markTable, charPoint);
            if (p >= 0) {
                return true;
            }
            p = ~p;
            if (p >= markTable.Length) {
                return false;
            }
            return (p & 1) != 0;
        }

        private static readonly int[] markTable = new int[] { 0x00300, 0x0036F, 0x00483, 0x00489, 0x00591, 0x005BD
            , 0x005BF, 0x005BF, 0x005C1, 0x005C2, 0x005C4, 0x005C5, 0x005C7, 0x005C7, 0x00610, 0x0061A, 0x0064B, 0x0065F
            , 0x00670, 0x00670, 0x006D6, 0x006DC, 0x006DF, 0x006E4, 0x006E7, 0x006E8, 0x006EA, 0x006ED, 0x00711, 0x00711
            , 0x00730, 0x0074A, 0x007A6, 0x007B0, 0x007EB, 0x007F3, 0x00816, 0x00819, 0x0081B, 0x00823, 0x00825, 0x00827
            , 0x00829, 0x0082D, 0x00859, 0x0085B, 0x008E4, 0x00903, 0x0093A, 0x0093C, 0x0093E, 0x0094F, 0x00951, 0x00957
            , 0x00962, 0x00963, 0x00981, 0x00983, 0x009BC, 0x009BC, 0x009BE, 0x009CD, 0x009D7, 0x009D7, 0x009E2, 0x009E3
            , 0x00A01, 0x00A03, 0x00A3C, 0x00A51, 0x00A70, 0x00A71, 0x00A75, 0x00A83, 0x00ABC, 0x00ABC, 0x00ABE, 0x00ACD
            , 0x00AE2, 0x00AE3, 0x00B01, 0x00B03, 0x00B3C, 0x00B3C, 0x00B3E, 0x00B57, 0x00B62, 0x00B63, 0x00B82, 0x00B82
            , 0x00BBE, 0x00BCD, 0x00BD7, 0x00BD7, 0x00C00, 0x00C03, 0x00C3E, 0x00C56, 0x00C62, 0x00C63, 0x00C81, 0x00C83
            , 0x00CBC, 0x00CBC, 0x00CBE, 0x00CD6, 0x00CE2, 0x00CE3, 0x00D01, 0x00D03, 0x00D3E, 0x00D4D, 0x00D57, 0x00D57
            , 0x00D62, 0x00D63, 0x00D82, 0x00D83, 0x00DCA, 0x00DDF, 0x00DF2, 0x00DF3, 0x00E31, 0x00E31, 0x00E34, 0x00E3A
            , 0x00E47, 0x00E4E, 0x00EB1, 0x00EB1, 0x00EB4, 0x00EBC, 0x00EC8, 0x00ECD, 0x00F18, 0x00F19, 0x00F35, 0x00F35
            , 0x00F37, 0x00F37, 0x00F39, 0x00F39, 0x00F3E, 0x00F3F, 0x00F71, 0x00F84, 0x00F86, 0x00F87, 0x00F8D, 0x00FBC
            , 0x00FC6, 0x00FC6, 0x0102B, 0x0103E, 0x01056, 0x01059, 0x0105E, 0x01060, 0x01062, 0x01064, 0x01067, 0x0106D
            , 0x01071, 0x01074, 0x01082, 0x0108D, 0x0108F, 0x0108F, 0x0109A, 0x0109D, 0x0135D, 0x0135F, 0x01712, 0x01714
            , 0x01732, 0x01734, 0x01752, 0x01753, 0x01772, 0x01773, 0x017B4, 0x017D3, 0x017DD, 0x017DD, 0x0180B, 0x0180D
            , 0x018A9, 0x018A9, 0x01920, 0x0193B, 0x019B0, 0x019C0, 0x019C8, 0x019C9, 0x01A17, 0x01A1B, 0x01A55, 0x01A7F
            , 0x01AB0, 0x01B04, 0x01B34, 0x01B44, 0x01B6B, 0x01B73, 0x01B80, 0x01B82, 0x01BA1, 0x01BAD, 0x01BE6, 0x01BF3
            , 0x01C24, 0x01C37, 0x01CD0, 0x01CD2, 0x01CD4, 0x01CE8, 0x01CED, 0x01CED, 0x01CF2, 0x01CF4, 0x01CF8, 0x01CF9
            , 0x01DC0, 0x01DFF, 0x020D0, 0x020F0, 0x02CEF, 0x02CF1, 0x02D7F, 0x02D7F, 0x02DE0, 0x02DFF, 0x0302A, 0x0302F
            , 0x03099, 0x0309A, 0x0A66F, 0x0A672, 0x0A674, 0x0A67D, 0x0A69F, 0x0A69F, 0x0A6F0, 0x0A6F1, 0x0A802, 0x0A802
            , 0x0A806, 0x0A806, 0x0A80B, 0x0A80B, 0x0A823, 0x0A827, 0x0A880, 0x0A881, 0x0A8B4, 0x0A8C4, 0x0A8E0, 0x0A8F1
            , 0x0A926, 0x0A92D, 0x0A947, 0x0A953, 0x0A980, 0x0A983, 0x0A9B3, 0x0A9C0, 0x0A9E5, 0x0A9E5, 0x0AA29, 0x0AA36
            , 0x0AA43, 0x0AA43, 0x0AA4C, 0x0AA4D, 0x0AA7B, 0x0AA7D, 0x0AAB0, 0x0AAB0, 0x0AAB2, 0x0AAB4, 0x0AAB7, 0x0AAB8
            , 0x0AABE, 0x0AABF, 0x0AAC1, 0x0AAC1, 0x0AAEB, 0x0AAEF, 0x0AAF5, 0x0AAF6, 0x0ABE3, 0x0ABEA, 0x0ABEC, 0x0ABED
            , 0x0FB1E, 0x0FB1E, 0x0FE00, 0x0FE0F, 0x0FE20, 0x0FE2D, 0x101FD, 0x101FD, 0x102E0, 0x102E0, 0x10376, 0x1037A
            , 0x10A01, 0x10A0F, 0x10A38, 0x10A3F, 0x10AE5, 0x10AE6, 0x11000, 0x11002, 0x11038, 0x11046, 0x1107F, 0x11082
            , 0x110B0, 0x110BA, 0x11100, 0x11102, 0x11127, 0x11134, 0x11173, 0x11173, 0x11180, 0x11182, 0x111B3, 0x111C0
            , 0x1122C, 0x11237, 0x112DF, 0x112EA, 0x11301, 0x11303, 0x1133C, 0x1133C, 0x1133E, 0x11357, 0x11362, 0x11374
            , 0x114B0, 0x114C3, 0x115AF, 0x115C0, 0x11630, 0x11640, 0x116AB, 0x116B7, 0x16AF0, 0x16AF4, 0x16B30, 0x16B36
            , 0x16F51, 0x16F92, 0x1BC9D, 0x1BC9E, 0x1D165, 0x1D169, 0x1D16D, 0x1D172, 0x1D17B, 0x1D182, 0x1D185, 0x1D18B
            , 0x1D1AA, 0x1D1AD, 0x1D242, 0x1D244, 0x1E8D0, 0x1E8D6, 0xE0100, 0xE01EF };
    }
}
