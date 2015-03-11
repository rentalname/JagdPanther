﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JagdPanther
{
	public static class ExtensionMethods
	{
		public static DateTime LastTime { get;set; }

		public static void CheckApi(this object obj)
		{
            if (DateTimeOffset.Now.ToUnixTimeSeconds() - new DateTimeOffset(LastTime).ToUnixTimeSeconds() >= 2)
            {
                LastTime = DateTime.Now;
            }
            else
				throw new InvalidOperationException();
		}
	}
}
