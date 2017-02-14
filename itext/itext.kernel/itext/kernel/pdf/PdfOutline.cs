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
using System;
using System.Collections.Generic;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Navigation;

namespace iText.Kernel.Pdf {
    /// <summary>
    /// Document outline object
    /// See ISO-320001, 12.3.3 Document Outline.
    /// </summary>
    public class PdfOutline {
        /// <summary>A flag for displaying the outline item’s text with italic font.</summary>
        public static int FLAG_ITALIC = 1;

        /// <summary>A flag for displaying the outline item’s text with bold font.</summary>
        public static int FLAG_BOLD = 2;

        private IList<iText.Kernel.Pdf.PdfOutline> children = new List<iText.Kernel.Pdf.PdfOutline>();

        private String title;

        private PdfDictionary content;

        private PdfDestination destination;

        private iText.Kernel.Pdf.PdfOutline parent;

        private PdfDocument pdfDoc;

        /// <summary>Create instance of document outline.</summary>
        /// <param name="title">the text that shall be displayed on the screen for this item.</param>
        /// <param name="content">Outline dictionary</param>
        /// <param name="pdfDocument">
        /// 
        /// <see cref="PdfDocument"/>
        /// the outline belongs to.
        /// </param>
        [System.ObsoleteAttribute(@"Use PdfCatalog.GetOutlines(bool) instead.")]
        public PdfOutline(String title, PdfDictionary content, PdfDocument pdfDocument) {
            this.title = title;
            this.content = content;
            this.pdfDoc = pdfDocument;
        }

        /// <summary>Create instance of document outline.</summary>
        /// <param name="title">the text that shall be displayed on the screen for this item.</param>
        /// <param name="content">Outline dictionary</param>
        /// <param name="parent">parent outline.</param>
        [System.ObsoleteAttribute(@"Use PdfCatalog.GetNextItem(PdfDictionary, PdfOutline, System.Collections.Generic.IDictionary{K, V}) ,AddOutline(System.String, int) and AddOutline(System.String) instead."
            )]
        public PdfOutline(String title, PdfDictionary content, iText.Kernel.Pdf.PdfOutline parent) {
            this.title = title;
            this.content = content;
            this.parent = parent;
            this.pdfDoc = parent.pdfDoc;
            content.MakeIndirect(parent.pdfDoc);
        }

        /// <summary>This constructor creates root outline in the document.</summary>
        /// <param name="doc">
        /// 
        /// <see cref="PdfDocument"/>
        /// </param>
        [System.ObsoleteAttribute(@"Use PdfCatalog.GetOutlines(bool) instead.")]
        protected internal PdfOutline(PdfDocument doc) {
            content = new PdfDictionary();
            content.Put(PdfName.Type, PdfName.Outlines);
            this.pdfDoc = doc;
            content.MakeIndirect(doc);
            doc.GetCatalog().AddRootOutline(this);
        }

        /// <summary>Gets title of the outline.</summary>
        /// <returns>String value.</returns>
        public virtual String GetTitle() {
            return title;
        }

        /// <summary>
        /// Sets title of the outline with
        /// <see cref="iText.IO.Font.PdfEncodings.UNICODE_BIG"/>
        /// encoding,
        /// <c>Title</c>
        /// key.
        /// </summary>
        /// <param name="title">String value.</param>
        public virtual void SetTitle(String title) {
            this.title = title;
            this.content.Put(PdfName.Title, new PdfString(title, PdfEncodings.UNICODE_BIG));
        }

        /// <summary>
        /// Sets color for the outline entry’s text,
        /// <c>C</c>
        /// key.
        /// </summary>
        /// <param name="color">
        /// 
        /// <see cref="iText.Kernel.Colors.Color"/>
        /// </param>
        public virtual void SetColor(Color color) {
            content.Put(PdfName.C, new PdfArray(color.GetColorValue()));
        }

        /// <summary>
        /// Sets text style for the outline entry’s text,
        /// <c>F</c>
        /// key.
        /// </summary>
        /// <param name="style">
        /// Could be either
        /// <see cref="FLAG_BOLD"/>
        /// or
        /// <see cref="FLAG_ITALIC"/>
        /// . Default value is
        /// <c>0</c>
        /// .
        /// </param>
        public virtual void SetStyle(int style) {
            if (style == FLAG_BOLD || style == FLAG_ITALIC) {
                content.Put(PdfName.F, new PdfNumber(style));
            }
        }

        /// <summary>Gets content dictionary.</summary>
        /// <returns>
        /// 
        /// <see cref="PdfDictionary"/>
        /// .
        /// </returns>
        public virtual PdfDictionary GetContent() {
            return content;
        }

        /// <summary>Gets list of children outlines.</summary>
        /// <returns>
        /// List of
        /// <see cref="PdfOutline"/>
        /// .
        /// </returns>
        public virtual IList<iText.Kernel.Pdf.PdfOutline> GetAllChildren() {
            return children;
        }

        /// <summary>Gets parent outline.</summary>
        /// <returns>
        /// 
        /// <see cref="PdfOutline"/>
        /// .
        /// </returns>
        public virtual iText.Kernel.Pdf.PdfOutline GetParent() {
            return parent;
        }

        /// <summary>
        /// Gets
        /// <see cref="iText.Kernel.Pdf.Navigation.PdfDestination"/>
        /// .
        /// </summary>
        /// <returns>
        /// 
        /// <see cref="iText.Kernel.Pdf.Navigation.PdfDestination"/>
        /// .
        /// </returns>
        public virtual PdfDestination GetDestination() {
            return destination;
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Navigation.PdfDestination"/>
        /// for the outline,
        /// <c>Dest</c>
        /// key.
        /// </summary>
        /// <param name="destination">
        /// instance of
        /// <see cref="iText.Kernel.Pdf.Navigation.PdfDestination"/>
        /// .
        /// </param>
        public virtual void AddDestination(PdfDestination destination) {
            SetDestination(destination);
            content.Put(PdfName.Dest, destination.GetPdfObject());
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Action.PdfAction"/>
        /// for the outline,
        /// <c>A</c>
        /// key.
        /// </summary>
        /// <param name="action">
        /// instance of
        /// <see cref="iText.Kernel.Pdf.Action.PdfAction"/>
        /// .
        /// </param>
        public virtual void AddAction(PdfAction action) {
            content.Put(PdfName.A, action.GetPdfObject());
        }

        /// <summary>Defines if the outline needs to be closed or not.</summary>
        /// <remarks>
        /// Defines if the outline needs to be closed or not.
        /// By default, outlines are open.
        /// </remarks>
        /// <param name="open">if false, the outline will be closed by default</param>
        public virtual void SetOpen(bool open) {
            if (!open) {
                content.Put(PdfName.Count, new PdfNumber(-1));
            }
            else {
                if (children.Count > 0) {
                    content.Put(PdfName.Count, new PdfNumber(children.Count));
                }
                else {
                    content.Remove(PdfName.Count);
                }
            }
        }

        /// <summary>
        /// Adds a new
        /// <c>PdfOutline</c>
        /// with specified parameters as a child to existing
        /// <c>PdfOutline</c>
        /// and put it to specified position in the existing
        /// <c>PdfOutline</c>
        /// children list.
        /// </summary>
        /// <param name="title">an outline title</param>
        /// <param name="position">
        /// a position in the current outline child List where a new outline should be added.
        /// If the position equals -1, then the outline will be put in the end of children list.
        /// </param>
        /// <returns>just created outline</returns>
        public virtual iText.Kernel.Pdf.PdfOutline AddOutline(String title, int position) {
            if (position == -1) {
                position = children.Count;
            }
            PdfDictionary dictionary = new PdfDictionary();
            iText.Kernel.Pdf.PdfOutline outline = new iText.Kernel.Pdf.PdfOutline(title, dictionary, this);
            dictionary.Put(PdfName.Title, new PdfString(title, PdfEncodings.UNICODE_BIG));
            dictionary.Put(PdfName.Parent, content);
            if (children.Count > 0) {
                if (position != 0) {
                    PdfDictionary prevContent = children[position - 1].GetContent();
                    dictionary.Put(PdfName.Prev, prevContent);
                    prevContent.Put(PdfName.Next, dictionary);
                }
                if (position != children.Count) {
                    PdfDictionary nextContent = children[position].GetContent();
                    dictionary.Put(PdfName.Next, nextContent);
                    nextContent.Put(PdfName.Prev, dictionary);
                }
            }
            if (position == 0) {
                content.Put(PdfName.First, dictionary);
            }
            if (position == children.Count) {
                content.Put(PdfName.Last, dictionary);
            }
            PdfNumber count = this.content.GetAsNumber(PdfName.Count);
            if (count == null || count.GetValue() != -1) {
                content.Put(PdfName.Count, new PdfNumber(children.Count + 1));
            }
            children.Add(position, outline);
            return outline;
        }

        /// <summary>
        /// Adds an
        /// <c>PdfOutline</c>
        /// as a child to existing
        /// <c>PdfOutline</c>
        /// and put it in the end of the existing
        /// <c>PdfOutline</c>
        /// children list.
        /// </summary>
        /// <param name="title">an outline title</param>
        /// <returns>just created outline</returns>
        public virtual iText.Kernel.Pdf.PdfOutline AddOutline(String title) {
            return AddOutline(title, -1);
        }

        /// <summary>
        /// Adds an
        /// <c>PdfOutline</c>
        /// as a child to existing
        /// <c>PdfOutline</c>
        /// and put it to the end of the existing
        /// <c>PdfOutline</c>
        /// children list.
        /// </summary>
        /// <param name="outline">an outline to add.</param>
        /// <returns>just created outline</returns>
        public virtual iText.Kernel.Pdf.PdfOutline AddOutline(iText.Kernel.Pdf.PdfOutline outline) {
            iText.Kernel.Pdf.PdfOutline newOutline = AddOutline(outline.GetTitle());
            newOutline.AddDestination(outline.GetDestination());
            IList<iText.Kernel.Pdf.PdfOutline> children = outline.GetAllChildren();
            foreach (iText.Kernel.Pdf.PdfOutline child in children) {
                newOutline.AddOutline(child);
            }
            return newOutline;
        }

        /// <summary>Clear list of children.</summary>
        internal virtual void Clear() {
            children.Clear();
        }

        /// <summary>
        /// Sets
        /// <see cref="iText.Kernel.Pdf.Navigation.PdfDestination"/>
        /// .
        /// </summary>
        /// <param name="destination">
        /// instance of
        /// <see cref="iText.Kernel.Pdf.Navigation.PdfDestination"/>
        /// .
        /// </param>
        internal virtual void SetDestination(PdfDestination destination) {
            this.destination = destination;
        }

        /// <summary>Remove this outline from the document.</summary>
        internal virtual void RemoveOutline() {
            PdfName type = content.GetAsName(PdfName.Type);
            if (type != null && type.Equals(PdfName.Outlines)) {
                pdfDoc.GetCatalog().Remove(PdfName.Outlines);
                return;
            }
            iText.Kernel.Pdf.PdfOutline parent = this.parent;
            IList<iText.Kernel.Pdf.PdfOutline> children = parent.children;
            children.Remove(this);
            PdfDictionary parentContent = parent.content;
            if (children.Count > 0) {
                parentContent.Put(PdfName.First, children[0].content);
                parentContent.Put(PdfName.Last, children[children.Count - 1].content);
            }
            else {
                parent.RemoveOutline();
                return;
            }
            PdfDictionary next = content.GetAsDictionary(PdfName.Next);
            PdfDictionary prev = content.GetAsDictionary(PdfName.Prev);
            if (prev != null) {
                if (next != null) {
                    prev.Put(PdfName.Next, next);
                    next.Put(PdfName.Prev, prev);
                }
                else {
                    prev.Remove(PdfName.Next);
                }
            }
            else {
                if (next != null) {
                    next.Remove(PdfName.Prev);
                }
            }
        }
    }
}
