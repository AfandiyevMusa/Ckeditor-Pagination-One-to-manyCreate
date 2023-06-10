using System;
using Fiorello.Models;

namespace Fiorello.ViewModels
{
	public class SliderVM
	{
        public IEnumerable<Slider> Sliders { get; set; }
        public SlidersInfo SlidersInfo { get; set; }
    }
}

