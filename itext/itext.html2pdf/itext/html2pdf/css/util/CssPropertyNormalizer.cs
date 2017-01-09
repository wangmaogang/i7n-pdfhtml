/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2017 iText Group NV
    Authors: iText Software.

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
    address: sales@itextpdf.com */
using System;
using System.Text;
using iText.IO.Log;

namespace iText.Html2pdf.Css.Util {
    internal class CssPropertyNormalizer {
        public static String Normalize(String str) {
            StringBuilder buffer = new StringBuilder();
            int segmentStart = 0;
            for (int i = 0; i < str.Length; ++i) {
                if (str[i] == '\\') {
                    ++i;
                }
                else {
                    if (str[i] == '\'' || str[i] == '"') {
                        AppendAndFormatSegment(buffer, str, segmentStart, i + 1);
                        segmentStart = i = AppendQuoteContent(buffer, str, i + 1, str[i]);
                    }
                }
            }
            if (segmentStart < str.Length) {
                AppendAndFormatSegment(buffer, str, segmentStart, str.Length);
            }
            return buffer.ToString();
        }

        private static void AppendAndFormatSegment(StringBuilder buffer, String source, int start, int end) {
            String[] parts = iText.IO.Util.StringUtil.Split(source.JSubstring(start, end), "\\s");
            StringBuilder sb = new StringBuilder();
            foreach (String part in parts) {
                if (part.Length > 0) {
                    if (sb.Length > 0 && !TrimSpaceAfter(sb[sb.Length - 1]) && !TrimSpaceBefore(part[0])) {
                        sb.Append(" ");
                    }
                    sb.Append(part.ToLowerInvariant());
                }
            }
            buffer.Append(sb);
        }

        private static int AppendQuoteContent(StringBuilder buffer, String source, int start, char endQuoteSymbol) {
            int end = FindNextUnescapedChar(source, endQuoteSymbol, start);
            if (end == -1) {
                end = source.Length;
                LoggerFactory.GetLogger(typeof(CssPropertyNormalizer)).Warn(String.Format(iText.Html2pdf.LogMessageConstant
                    .QUOTE_IS_NOT_CLOSED_IN_CSS_EXPRESSION, source));
            }
            buffer.JAppend(source, start, end);
            return end;
        }

        private static int FindNextUnescapedChar(String source, char ch, int startIndex) {
            int symbolPos = source.IndexOf(ch, startIndex);
            if (symbolPos == -1) {
                return -1;
            }
            int afterNoneEscapePos = symbolPos;
            while (afterNoneEscapePos > 0 && source[afterNoneEscapePos - 1] == '\\') {
                --afterNoneEscapePos;
            }
            return (symbolPos - afterNoneEscapePos) % 2 == 0 ? symbolPos : FindNextUnescapedChar(source, ch, symbolPos
                 + 1);
        }

        private static bool TrimSpaceAfter(char ch) {
            return ch == ',' || ch == '(';
        }

        private static bool TrimSpaceBefore(char ch) {
            return ch == ',' || ch == ')';
        }
    }
}