using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressTrack.Models {
	class Express {
		public int Id { get; set; }
		public string Name { get; set; }
		public Express(int id, string name) {
			Id = id;
			Name = name;
		}
	}
}
