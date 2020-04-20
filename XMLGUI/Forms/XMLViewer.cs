using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using XMLGUI.Forms;
using XMLGUI.EventsLib;
using XMLGUI.DataClass;

namespace XMLGUI
{
    public partial class XMLViewer : Form
    {
        private Controller crt;

        public XMLViewer()
        {
            InitializeComponent();
            this.crt = new Controller();
        }

        private void OnSetFilterClick(object sender, EventArgs e)
        {
            FilterProperties setFilterForm = new FilterProperties();
            setFilterForm.FilterChangeEvent += new EventHandler<FilterChangeEventArgs>(this.OnFilterChangeEvent);
            setFilterForm.ShowDialog();
        }

        public void OnFilterChangeEvent(object sender, FilterChangeEventArgs e)
        {
            RedrawTree(crt.Filter(e.Name, e.Param));
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog {
                DefaultExt = "xml",
                Filter = "Library db (*.xml)|*.xml"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                int res = crt.LoadFile(fileDialog.FileName);
                if (res == 0)
                {
                    RedrawTree(crt.readers);
                    setFilterToolStripMenuItem.Enabled = true;
                    clearToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem1.Enabled = true;
                    deleteToolStripMenuItem.Enabled = true;
                    readerToolStripMenuItem.Enabled = true;
                    bookToolStripMenuItem.Enabled = true;
                } else if (res == 1)
                    MessageBox.Show("Файл БД поврежден", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (res == 2)
                    MessageBox.Show("Файл БД поврежден", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void RedrawTree(List<Reader> readers)
        {
            workTree.Nodes.Clear();
            foreach (Reader reader in readers)
            {
                TreeNode node = new TreeNode(reader.ToString()) {
                    Tag = reader,
                    Name = reader.GetHashCode().ToString()
                };

                workTree.Nodes.Add(node);
                foreach (Book book in reader.books)
                {
                    TreeNode bnode = new TreeNode(book.ToString()) {
                        Tag = book,
                        Name = book.GetHashCode().ToString()
                    };
                    node.Nodes.Add(bnode);
                    bnode.Nodes.Add(new TreeNode("Выдана " + book.dateOut.ToString("dd.MM.yyyy")));
                    bnode.Nodes.Add(new TreeNode("Срок выдачи " + book.days.ToString()));
                    bnode.Nodes.Add(new TreeNode("Год издания " + book.year.ToString()));
                    bnode.Nodes.Add(new TreeNode("Издательство " + book.izdat));
                    bnode.Nodes.Add(new TreeNode("Стоимость " + book.izdat.ToString()));
                }
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e) => RedrawTree(crt.readers);

        private void DeleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var node = workTree.SelectedNode;
            if (node == null)
            {
                MessageBox.Show("Выберите элемент для удаления", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (node.Tag is Reader || node.Tag is Book)
                crt.DeleteNod(node.Tag);
            else
            {
                MessageBox.Show("Удалять можно только Читателя и Книгу", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            workTree.SelectedNode.Remove();
        }

        private void SaveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            crt.Save();
            MessageBox.Show("Сохранение завершено", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void ReaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertReader insertReader = new InsertReader();
            insertReader.InsertReaderEvent += (s, ev) => {
                crt.AddReader(new Reader() {
                    number = ev.Num,
                    sname = ev.SName,
                    books = new List<Book>()
                });
                RedrawTree(crt.readers);
            };
            insertReader.ShowDialog();
        }

        private void BookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (workTree.SelectedNode == null || (string)workTree.SelectedNode.Tag != "reader")
            {
                MessageBox.Show("Выберите читателя!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            InsertBook insertBook = new InsertBook();
            insertBook.InsertBookEvent += (s, ev) => {
                crt.AddBook((Reader)workTree.SelectedNode.Tag, new Book() {
                    dateOut = ev.DateOut,
                    days = ev.Days,
                    author = ev.Author,
                    name = ev.Name,
                    year = ev.Year,
                    izdat = ev.Izdat,
                    price = ev.Price
                });
                RedrawTree(crt.readers);
            };
            insertBook.ShowDialog();
        }
    }
}
