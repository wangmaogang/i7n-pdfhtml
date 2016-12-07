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
using System.Collections.Generic;
using iText.Html2pdf.Css;
using iText.Html2pdf.Css.Resolve.Shorthand;
using iText.Html2pdf.Css.Util;
using iText.IO.Log;

namespace iText.Html2pdf.Css.Resolve.Shorthand.Impl {
    public class BackgroundShorthandResolver : IShorthandResolver {
        private const int UNDEFINED_TYPE = -1;

        private const int BACKGROUND_COLOR_TYPE = 0;

        private const int BACKGROUND_IMAGE_TYPE = 1;

        private const int BACKGROUND_POSITION_TYPE = 2;

        private const int BACKGROUND_POSITION_OR_SIZE_TYPE = 3;

        private const int BACKGROUND_REPEAT_TYPE = 4;

        private const int BACKGROUND_ORIGIN_OR_CLIP_TYPE = 5;

        private const int BACKGROUND_CLIP_TYPE = 6;

        private const int BACKGROUND_ATTACHMENT_TYPE = 7;

        // might have the same type, but position always precedes size
        // have the same possible values but apparently origin values precedes clip value
        // With CSS3, you can apply multiple backgrounds to elements. These are layered atop one another
        // with the first background you provide on top and the last background listed in the back. Only
        // the last background can include a background color.
        public virtual IList<CssDeclaration> ResolveShorthand(String shorthandExpression) {
            if (CssConstants.INITIAL.Equals(shorthandExpression) || CssConstants.INHERIT.Equals(shorthandExpression)) {
                return iText.IO.Util.JavaUtil.ArraysAsList(new CssDeclaration(CssConstants.BACKGROUND_COLOR, shorthandExpression
                    ), new CssDeclaration(CssConstants.BACKGROUND_IMAGE, shorthandExpression), new CssDeclaration(CssConstants
                    .BACKGROUND_POSITION, shorthandExpression), new CssDeclaration(CssConstants.BACKGROUND_SIZE, shorthandExpression
                    ), new CssDeclaration(CssConstants.BACKGROUND_REPEAT, shorthandExpression), new CssDeclaration(CssConstants
                    .BACKGROUND_ORIGIN, shorthandExpression), new CssDeclaration(CssConstants.BACKGROUND_CLIP, shorthandExpression
                    ), new CssDeclaration(CssConstants.BACKGROUND_ATTACHMENT, shorthandExpression));
            }
            IList<String> commaSeparatedExpressions = SplitMultipleBackgrounds(shorthandExpression);
            // TODO ignore multiple backgrounds at the moment
            String backgroundExpression = commaSeparatedExpressions[0];
            String[] resolvedProps = new String[8];
            String[] props = iText.IO.Util.StringUtil.Split(backgroundExpression, "\\s+");
            bool slashEncountered = false;
            foreach (String value in props) {
                int slashCharInd = value.IndexOf('/');
                if (slashCharInd > 0) {
                    slashEncountered = true;
                    String value1 = value.JSubstring(0, slashCharInd);
                    String value2 = value.JSubstring(slashCharInd + 1, value.Length);
                    PutPropertyBasedOnType(ResolvePropertyType(value1), value1, resolvedProps, false);
                    PutPropertyBasedOnType(ResolvePropertyType(value2), value2, resolvedProps, true);
                }
                else {
                    PutPropertyBasedOnType(ResolvePropertyType(value), value, resolvedProps, slashEncountered);
                }
            }
            for (int i = 0; i < resolvedProps.Length; ++i) {
                if (resolvedProps[i] == null) {
                    resolvedProps[i] = CssConstants.INITIAL;
                }
            }
            IList<CssDeclaration> cssDeclarations = iText.IO.Util.JavaUtil.ArraysAsList(new CssDeclaration(CssConstants
                .BACKGROUND_COLOR, resolvedProps[BACKGROUND_COLOR_TYPE]), new CssDeclaration(CssConstants.BACKGROUND_IMAGE
                , resolvedProps[BACKGROUND_IMAGE_TYPE]), new CssDeclaration(CssConstants.BACKGROUND_POSITION, resolvedProps
                [BACKGROUND_POSITION_TYPE]), new CssDeclaration(CssConstants.BACKGROUND_SIZE, resolvedProps[BACKGROUND_POSITION_OR_SIZE_TYPE
                ]), new CssDeclaration(CssConstants.BACKGROUND_REPEAT, resolvedProps[BACKGROUND_REPEAT_TYPE]), new CssDeclaration
                (CssConstants.BACKGROUND_ORIGIN, resolvedProps[BACKGROUND_ORIGIN_OR_CLIP_TYPE]), new CssDeclaration(CssConstants
                .BACKGROUND_CLIP, resolvedProps[BACKGROUND_CLIP_TYPE]), new CssDeclaration(CssConstants.BACKGROUND_ATTACHMENT
                , resolvedProps[BACKGROUND_ATTACHMENT_TYPE]));
            return cssDeclarations;
        }

        private int ResolvePropertyType(String value) {
            if (value.Contains("url(") || CssConstants.NONE.Equals(value)) {
                return BACKGROUND_IMAGE_TYPE;
            }
            else {
                if (CssConstants.BACKGROUND_REPEAT_VALUES.Contains(value)) {
                    return BACKGROUND_REPEAT_TYPE;
                }
                else {
                    if (CssConstants.BACKGROUND_ATTACHMENT_VALUES.Contains(value)) {
                        return BACKGROUND_ATTACHMENT_TYPE;
                    }
                    else {
                        if (CssConstants.BACKGROUND_POSITION_VALUES.Contains(value)) {
                            return BACKGROUND_POSITION_TYPE;
                        }
                        else {
                            if (CssUtils.IsNumericValue(value) || CssUtils.IsMetricValue(value) || CssUtils.IsRelativeValue(value)) {
                                return BACKGROUND_POSITION_OR_SIZE_TYPE;
                            }
                            else {
                                if (CssConstants.BACKGROUND_SIZE_VALUES.Contains(value)) {
                                    return BACKGROUND_POSITION_OR_SIZE_TYPE;
                                }
                                else {
                                    if (CssUtils.IsColorProperty(value)) {
                                        return BACKGROUND_COLOR_TYPE;
                                    }
                                    else {
                                        if (CssConstants.BACKGROUND_ORIGIN_OR_CLIP_VALUES.Contains(value)) {
                                            return BACKGROUND_ORIGIN_OR_CLIP_TYPE;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return UNDEFINED_TYPE;
        }

        private void PutPropertyBasedOnType(int type, String value, String[] resolvedProps, bool slashEncountered) {
            if (type == UNDEFINED_TYPE) {
                ILogger logger = LoggerFactory.GetLogger(typeof(BackgroundShorthandResolver));
                logger.Error(String.Format("Was not able to define one of the background CSS shorthand properties: {0}", value
                    ));
                return;
            }
            if (type == BACKGROUND_POSITION_OR_SIZE_TYPE && !slashEncountered) {
                type = BACKGROUND_POSITION_TYPE;
            }
            if (type == BACKGROUND_ORIGIN_OR_CLIP_TYPE && resolvedProps[BACKGROUND_ORIGIN_OR_CLIP_TYPE] != null) {
                type = BACKGROUND_CLIP_TYPE;
            }
            if ((type == BACKGROUND_POSITION_OR_SIZE_TYPE || type == BACKGROUND_POSITION_TYPE) && resolvedProps[type] 
                != null) {
                resolvedProps[type] += " " + value;
            }
            else {
                resolvedProps[type] = value;
            }
        }

        private IList<String> SplitMultipleBackgrounds(String shorthandExpression) {
            IList<String> commaSeparatedExpressions = new List<String>();
            bool isInsideParentheses = false;
            // in order to avoid split inside rgb/rgba color definition
            int prevStart = 0;
            for (int i = 0; i < shorthandExpression.Length; ++i) {
                if (shorthandExpression[i] == ',' && !isInsideParentheses) {
                    commaSeparatedExpressions.Add(shorthandExpression.JSubstring(prevStart, i));
                    prevStart = i + 1;
                }
                else {
                    if (shorthandExpression[i] == '(') {
                        isInsideParentheses = true;
                    }
                    else {
                        if (shorthandExpression[i] == ')') {
                            isInsideParentheses = false;
                        }
                    }
                }
            }
            if (commaSeparatedExpressions.IsEmpty()) {
                commaSeparatedExpressions.Add(shorthandExpression);
            }
            return commaSeparatedExpressions;
        }
    }
}