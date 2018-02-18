using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD2Store.Domain.Entities
{
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
    public class Cart
    {
        //create a list for cart items
        private List<CartLine> lineCollection = new List<CartLine>();

        //access the content of the cart
        public IEnumerable<CartLine> Lines
        {
            get
            {
                return lineCollection;
            }
        }

        // add an item to the cart
        public void AddItem(Product myproduct, int myquantity)
        {
            CartLine line = lineCollection.Where(p => p.Product.ProductID == myproduct.ProductID).FirstOrDefault();

            //if line does not exist, i.e 'line' returns null
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = myproduct,
                    Quantity = myquantity
                });
            }
            //if item already exists in the cart
            else
            {
                line.Quantity += myquantity;
            }
        }

        //remove an item from the cart
        public void RemoveItem(Product myproduct)
        {
            lineCollection.RemoveAll(p => p.Product.ProductID == myproduct.ProductID);
        }

        //compute the total cost of the cart
        public decimal ComputeTotalCost()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        //reset everything in cat
        public void Clear()
        {
            lineCollection.Clear();
        }
    }
}
