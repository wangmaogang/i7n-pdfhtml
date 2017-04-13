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
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css;
using iText.Html2pdf.Html;
using iText.Html2pdf.Html.Node;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Html2pdf.Attach.Impl.Tags {
    public class ImgTagWorker : ITagWorker {
        private ImgTagWorker.HtmlImage image;

        private String display;

        public ImgTagWorker(IElementNode element, ProcessorContext context) {
            PdfImageXObject imageXObject = context.GetResourceResolver().RetrieveImage(element.GetAttribute(AttributeConstants
                .SRC));
            if (imageXObject != null) {
                image = new ImgTagWorker.HtmlImage(this, imageXObject);
                String altText = element.GetAttribute(AttributeConstants.ALT);
                if (altText != null) {
                    image.SetAltText(altText);
                }
            }
            display = element.GetStyles() != null ? element.GetStyles().Get(CssConstants.DISPLAY) : null;
            // TODO this is a workaround for now to that image is not added as inline
            if (element.GetStyles() != null && CssConstants.ABSOLUTE.Equals(element.GetStyles().Get(CssConstants.POSITION
                ))) {
                display = CssConstants.BLOCK;
            }
        }

        public virtual void ProcessEnd(IElementNode element, ProcessorContext context) {
        }

        public virtual bool ProcessContent(String content, ProcessorContext context) {
            return false;
        }

        public virtual bool ProcessTagChild(ITagWorker childTagWorker, ProcessorContext context) {
            return false;
        }

        public virtual IPropertyContainer GetElementResult() {
            return image;
        }

        internal virtual String GetDisplay() {
            return display;
        }

        private class HtmlImage : Image {
            private double pxToPt = 0.75;

            public HtmlImage(ImgTagWorker _enclosing, PdfImageXObject xObject)
                : base(xObject) {
                this._enclosing = _enclosing;
            }

            // In iText by default we set image sizes (in points) exactly of the image height and width in pixels.
            public override float GetImageWidth() {
                return (float)(this.xObject.GetWidth() * this.pxToPt);
            }

            public override float GetImageHeight() {
                return (float)(this.xObject.GetHeight() * this.pxToPt);
            }

            private void SetAltText(String altText) {
                this.GetAccessibilityProperties().SetAlternateDescription(altText);
            }

            private readonly ImgTagWorker _enclosing;
        }
    }
}
