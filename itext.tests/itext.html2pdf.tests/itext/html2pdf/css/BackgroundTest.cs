/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
address: sales@itextpdf.com
*/
using System;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Utils;
using iText.Test.Attributes;

namespace iText.Html2pdf.Css {
    public class BackgroundTest : ExtendedHtmlConversionITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/html2pdf/css/BackgroundTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/html2pdf/css/BackgroundTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        [NUnit.Framework.Ignore("DEVSIX-1708")]
        public virtual void BackgroundSizeTest01() {
            HtmlConverter.ConvertToPdf(new FileInfo(sourceFolder + "backgroundsize01.html"), new FileInfo(destinationFolder
                 + "backgroundsize01.pdf"));
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "backgroundsize01.pdf"
                , sourceFolder + "cmp_backgroundsize01.pdf", destinationFolder));
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundAttachmentMarginRoot1Test() {
            // TODO DEVSIX-1708 support background-size
            ConvertToPdfAndCompare("backgroundAttachmentMarginRoot1", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundAttachmentMarginRoot2Test() {
            // TODO DEVSIX-1708 support background-size
            ConvertToPdfAndCompare("backgroundAttachmentMarginRoot2", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundColorBodyDisplayContentsTest() {
            // TODO DEVSIX-4445 support display: contents
            ConvertToPdfAndCompare("backgroundColorBodyDisplayContents", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundMarginHtmlTest() {
            ConvertToPdfAndCompare("backgroundMarginHtml", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        // TODO DEVSIX-4426 support rotateZ() - remove log message after fixing
        [LogMessage(iText.Html2pdf.LogMessageConstant.INVALID_CSS_PROPERTY_DECLARATION)]
        public virtual void BackgroundTransformedRootTest() {
            ConvertToPdfAndCompare("backgroundTransformedRoot", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundWillChangeRootTest() {
            // TODO DEVSIX-4448 support will-change CSS property
            ConvertToPdfAndCompare("backgroundWillChangeRoot", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundSoloImageTest() {
            ConvertToPdfAndCompare("background_solo_image", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.StyledXmlParser.LogMessageConstant.ONLY_THE_LAST_BACKGROUND_CAN_INCLUDE_BACKGROUND_COLOR
            )]
        public virtual void BackgroundImageAndColorNotLastTest() {
            ConvertToPdfAndCompare("background_image_and_color_not_last", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.Html2pdf.LogMessageConstant.INVALID_CSS_PROPERTY_DECLARATION)]
        public virtual void BackgroundImageAndColorsTest() {
            ConvertToPdfAndCompare("background_image_and_colors", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundSoloImageWithNoRepeatTest() {
            ConvertToPdfAndCompare("background_solo_image_with_no_repeat", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundSoloImageWithNoRepeatAndColorTest() {
            ConvertToPdfAndCompare("background_solo_image_with_no_repeat_and_color", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundMultiImageTest() {
            ConvertToPdfAndCompare("background_multi_image", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundTransparentAndNotTransparentImagesTest() {
            ConvertToPdfAndCompare("backgroundTransparentAndNotTransparentImages", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundTwoTransparentImagesAndColorTest() {
            ConvertToPdfAndCompare("backgroundTwoTransparentImagesAndColor", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundShorthandOnlyImageTest() {
            ConvertToPdfAndCompare("backgroundShorthandOnlyImage", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundShorthandImageRepeatAndColorTest() {
            ConvertToPdfAndCompare("backgroundShorthandImageRepeatAndColor", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundShorthandTwoImageWithRepeatAndColorTest() {
            ConvertToPdfAndCompare("backgroundShorthandTwoImageWithRepeatAndColor", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundShorthandThreeImagesWithOneRepeatTest() {
            ConvertToPdfAndCompare("backgroundShorthandThreeImagesWithOneRepeat", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundShorthandAndPropertyTest() {
            ConvertToPdfAndCompare("backgroundShorthandAndProperty", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundPropertyAndShorthandTest() {
            ConvertToPdfAndCompare("backgroundPropertyAndShorthand", sourceFolder, destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundSvgTest() {
            String testName = "backgroundSvgTest";
            HtmlConverter.ConvertToPdf(new FileInfo(sourceFolder + testName + ".xht"), new FileInfo(destinationFolder 
                + testName + ".pdf"));
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + testName + ".pdf", sourceFolder
                 + "cmp_" + testName + ".pdf", destinationFolder));
        }
    }
}
