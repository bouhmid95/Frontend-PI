using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
	public enum StockPosition { Row1_Pos1, Row1_Pos2, Row1_Pos3, Row2_Pos1, Row2_Pos2, Row2_Pos3, Row3_Pos1, Row3_Pos2, Row3_Pos3 }

	public class Stock
    {
		public int id { get; set; }
		public int qte { get; set; }
		public StockPosition stockPosition { get; set; }
		public Product product { get; set; }

	}
}