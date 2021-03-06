/*

This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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
using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Collection;
using iText.Kernel.Pdf.Xobject;

namespace iText.Kernel.Pdf.Filespec {
    public class PdfFileSpec : PdfObjectWrapper<PdfObject> {
        protected internal PdfFileSpec(PdfObject pdfObject)
            : base(pdfObject) {
        }

        public static iText.Kernel.Pdf.Filespec.PdfFileSpec WrapFileSpecObject(PdfObject fileSpecObject) {
            if (fileSpecObject != null) {
                if (fileSpecObject.IsString()) {
                    return new PdfStringFS((PdfString)fileSpecObject);
                }
                else {
                    if (fileSpecObject.IsDictionary()) {
                        return new PdfDictionaryFS((PdfDictionary)fileSpecObject);
                    }
                }
            }
            return null;
        }

        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateExternalFileSpec(PdfDocument doc, String filePath
            , PdfName afRelationshipValue) {
            PdfDictionary dict = new PdfDictionary();
            dict.Put(PdfName.Type, PdfName.Filespec);
            dict.Put(PdfName.F, new PdfString(filePath));
            dict.Put(PdfName.UF, new PdfString(filePath, PdfEncodings.UNICODE_BIG));
            if (afRelationshipValue != null) {
                dict.Put(PdfName.AFRelationship, afRelationshipValue);
            }
            else {
                dict.Put(PdfName.AFRelationship, PdfName.Unspecified);
            }
            return (iText.Kernel.Pdf.Filespec.PdfFileSpec)new iText.Kernel.Pdf.Filespec.PdfFileSpec(dict).MakeIndirect
                (doc);
        }

        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateExternalFileSpec(PdfDocument doc, String filePath
            ) {
            return CreateExternalFileSpec(doc, filePath, null);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc">PdfDocument to add the file to</param>
        /// <param name="fileStore">byte[] containing the file</param>
        /// <param name="description">file description</param>
        /// <param name="fileDisplay">actual file name stored in the pdf</param>
        /// <param name="mimeType">mime-type of the file</param>
        /// <param name="fileParameter">Pdfdictionary containing fil parameters</param>
        /// <param name="afRelationshipValue">AFRelationship key value, @see AFRelationshipValue. If <CODE>null</CODE>, @see AFRelationshipValue.Unspecified will be added.
        ///     </param>
        /// <returns>PdfFileSpec containing the file specification of the file as Pdfobject</returns>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, byte[] fileStore
            , String description, String fileDisplay, PdfName mimeType, PdfDictionary fileParameter, PdfName afRelationshipValue
            ) {
            PdfStream stream = (PdfStream)new PdfStream(fileStore).MakeIndirect(doc);
            PdfDictionary @params = new PdfDictionary();
            if (fileParameter != null) {
                @params.MergeDifferent(fileParameter);
            }
            if (!@params.ContainsKey(PdfName.ModDate)) {
                @params.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            }
            if (fileStore != null) {
                @params.Put(PdfName.Size, new PdfNumber(stream.GetBytes().Length));
            }
            stream.Put(PdfName.Params, @params);
            return CreateEmbeddedFileSpec(doc, stream, description, fileDisplay, mimeType, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc">PdfDocument to add the file to</param>
        /// <param name="fileStore">byte[] containing the file</param>
        /// <param name="fileDisplay">actual file name stored in the pdf</param>
        /// <param name="fileParameter">Pdfdictionary containing fil parameters</param>
        /// <param name="afRelationshipValue">AFRelationship key value, @see AFRelationshipValue. If <CODE>null</CODE>, @see AFRelationshipValue.Unspecified will be added.
        ///     </param>
        /// <returns>PdfFileSpec containing the file specification of the file as Pdfobject</returns>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, byte[] fileStore
            , String description, String fileDisplay, PdfDictionary fileParameter, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, fileStore, description, fileDisplay, null, fileParameter, afRelationshipValue
                );
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc">PdfDocument to add the file to</param>
        /// <param name="fileStore">byte[] containing the file</param>
        /// <param name="fileDisplay">actual file name stored in the pdf</param>
        /// <param name="fileParameter">Pdfdictionary containing fil parameters</param>
        /// <param name="afRelationshipValue">AFRelationship key value, @see AFRelationshipValue. If <CODE>null</CODE>, @see AFRelationshipValue.Unspecified will be added.
        ///     </param>
        /// <returns>PdfFileSpec containing the file specification of the file as Pdfobject</returns>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, byte[] fileStore
            , String fileDisplay, PdfDictionary fileParameter, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, fileStore, null, fileDisplay, null, fileParameter, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc">PdfDocument to add the file to</param>
        /// <param name="fileStore">byte[] containing the file</param>
        /// <param name="fileDisplay">actual file name stored in the pdf</param>
        /// <param name="afRelationshipValue">AFRelationship key value, @see AFRelationshipValue. If <CODE>null</CODE>, @see AFRelationshipValue.Unspecified will be added.
        ///     </param>
        /// <returns>PdfFileSpec containing the file specification of the file as Pdfobject</returns>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, byte[] fileStore
            , String fileDisplay, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, fileStore, null, fileDisplay, null, null, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc">PdfDocument to add the file to</param>
        /// <param name="fileStore">byte[] containing the file</param>
        /// <param name="description">file description</param>
        /// <param name="fileDisplay">actual file name stored in the pdf</param>
        /// <param name="afRelationshipValue">AFRelationship key value, @see AFRelationshipValue. If <CODE>null</CODE>, @see AFRelationshipValue.Unspecified will be added.
        ///     </param>
        /// <returns>PdfFileSpec containing the file specification of the file as Pdfobject</returns>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, byte[] fileStore
            , String description, String fileDisplay, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, fileStore, description, fileDisplay, null, null, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="filePath"/>
        /// <param name="description"/>
        /// <param name="fileDisplay"/>
        /// <param name="mimeType"/>
        /// <param name="fileParameter"/>
        /// <param name="afRelationshipValue"/>
        /// <exception cref="System.IO.IOException"/>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, String filePath
            , String description, String fileDisplay, PdfName mimeType, PdfDictionary fileParameter, PdfName afRelationshipValue
            ) {
            PdfStream stream = new PdfStream(doc, UrlUtil.OpenStream(UrlUtil.ToURL(filePath)));
            PdfDictionary @params = new PdfDictionary();
            if (fileParameter != null) {
                @params.MergeDifferent(fileParameter);
            }
            if (!@params.ContainsKey(PdfName.ModDate)) {
                @params.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            }
            stream.Put(PdfName.Params, @params);
            return CreateEmbeddedFileSpec(doc, stream, description, fileDisplay, mimeType, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="filePath"/>
        /// <param name="description"/>
        /// <param name="fileDisplay"/>
        /// <param name="mimeType"/>
        /// <param name="afRelationshipValue"/>
        /// <exception cref="System.IO.IOException"/>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, String filePath
            , String description, String fileDisplay, PdfName mimeType, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, filePath, description, fileDisplay, mimeType, null, afRelationshipValue
                );
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="filePath"/>
        /// <param name="description"/>
        /// <param name="fileDisplay"/>
        /// <param name="afRelationshipValue"/>
        /// <exception cref="System.IO.IOException"/>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, String filePath
            , String description, String fileDisplay, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, filePath, description, fileDisplay, null, null, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="filePath"/>
        /// <param name="fileDisplay"/>
        /// <param name="afRelationshipValue"/>
        /// <exception cref="System.IO.IOException"/>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, String filePath
            , String fileDisplay, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, filePath, null, fileDisplay, null, null, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="is"/>
        /// <param name="description"/>
        /// <param name="fileDisplay"/>
        /// <param name="mimeType"/>
        /// <param name="fileParameter"/>
        /// <param name="afRelationshipValue"/>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, Stream @is, String
             description, String fileDisplay, PdfName mimeType, PdfDictionary fileParameter, PdfName afRelationshipValue
            ) {
            PdfStream stream = new PdfStream(doc, @is);
            PdfDictionary @params = new PdfDictionary();
            if (fileParameter != null) {
                @params.MergeDifferent(fileParameter);
            }
            if (!@params.ContainsKey(PdfName.ModDate)) {
                @params.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            }
            stream.Put(PdfName.Params, @params);
            return CreateEmbeddedFileSpec(doc, stream, description, fileDisplay, mimeType, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="is"/>
        /// <param name="description"/>
        /// <param name="fileDisplay"/>
        /// <param name="mimeType"/>
        /// <param name="afRelationshipValue"/>
        public static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, Stream @is, String
             description, String fileDisplay, PdfName mimeType, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, @is, description, fileDisplay, mimeType, null, afRelationshipValue);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="stream"/>
        /// <param name="description"/>
        /// <param name="fileDisplay"/>
        /// <param name="mimeType"/>
        /// <param name="afRelationshipValue"/>
        private static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, PdfStream stream
            , String description, String fileDisplay, PdfName mimeType, PdfName afRelationshipValue) {
            PdfDictionary dict = new PdfDictionary();
            stream.Put(PdfName.Type, PdfName.EmbeddedFile);
            if (afRelationshipValue != null) {
                dict.Put(PdfName.AFRelationship, afRelationshipValue);
            }
            else {
                dict.Put(PdfName.AFRelationship, PdfName.Unspecified);
            }
            if (mimeType != null) {
                stream.Put(PdfName.Subtype, mimeType);
            }
            else {
                stream.Put(PdfName.Subtype, PdfName.ApplicationOctetStream);
            }
            if (description != null) {
                dict.Put(PdfName.Desc, new PdfString(description));
            }
            dict.Put(PdfName.Type, PdfName.Filespec);
            dict.Put(PdfName.F, new PdfString(fileDisplay));
            dict.Put(PdfName.UF, new PdfString(fileDisplay, PdfEncodings.UNICODE_BIG));
            PdfDictionary ef = new PdfDictionary();
            ef.Put(PdfName.F, stream);
            ef.Put(PdfName.UF, stream);
            dict.Put(PdfName.EF, ef);
            return (iText.Kernel.Pdf.Filespec.PdfFileSpec)new iText.Kernel.Pdf.Filespec.PdfFileSpec(dict).MakeIndirect
                (doc);
        }

        /// <summary>Embed a file to a PdfDocument.</summary>
        /// <param name="doc"/>
        /// <param name="stream"/>
        /// <param name="fileDisplay"/>
        /// <param name="afRelationshipValue"/>
        private static iText.Kernel.Pdf.Filespec.PdfFileSpec CreateEmbeddedFileSpec(PdfDocument doc, PdfStream stream
            , String description, String fileDisplay, PdfName afRelationshipValue) {
            return CreateEmbeddedFileSpec(doc, stream, description, fileDisplay, null, afRelationshipValue);
        }

        public virtual iText.Kernel.Pdf.Filespec.PdfFileSpec SetFileIdentifier(PdfArray fileIdentifier) {
            return Put(PdfName.ID, fileIdentifier);
        }

        public virtual PdfArray GetFileIdentifier() {
            return ((PdfDictionary)GetPdfObject()).GetAsArray(PdfName.ID);
        }

        public virtual iText.Kernel.Pdf.Filespec.PdfFileSpec SetVolatile(PdfBoolean isVolatile) {
            return Put(PdfName.Volatile, isVolatile);
        }

        public virtual PdfBoolean IsVolatile() {
            return ((PdfDictionary)GetPdfObject()).GetAsBoolean(PdfName.Volatile);
        }

        public virtual iText.Kernel.Pdf.Filespec.PdfFileSpec SetCollectionItem(PdfCollectionItem item) {
            return Put(PdfName.CI, item.GetPdfObject());
        }

        /// <summary>PDF 2.0.</summary>
        /// <remarks>PDF 2.0. Sets a stream object defining the thumbnail image for the file specification.</remarks>
        /// <param name="thumbnailImage">image used as a thumbnail</param>
        /// <returns>
        /// this
        /// <see cref="PdfFileSpec"/>
        /// instance
        /// </returns>
        public virtual iText.Kernel.Pdf.Filespec.PdfFileSpec SetThumbnailImage(PdfImageXObject thumbnailImage) {
            return Put(PdfName.Thumb, thumbnailImage.GetPdfObject());
        }

        /// <summary>PDF 2.0.</summary>
        /// <remarks>PDF 2.0. Gets a stream object defining the thumbnail image for the file specification.</remarks>
        /// <returns>image used as a thumbnail, or <code>null</code> if it is not set</returns>
        public virtual PdfImageXObject GetThumbnailImage() {
            PdfStream thumbnailStream = ((PdfDictionary)GetPdfObject()).GetAsStream(PdfName.Thumb);
            return thumbnailStream != null ? new PdfImageXObject(thumbnailStream) : null;
        }

        public virtual iText.Kernel.Pdf.Filespec.PdfFileSpec Put(PdfName key, PdfObject value) {
            ((PdfDictionary)GetPdfObject()).Put(key, value);
            SetModified();
            return this;
        }

        protected internal override bool IsWrappedObjectMustBeIndirect() {
            return true;
        }
    }
}
