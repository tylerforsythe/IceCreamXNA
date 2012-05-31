using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream.SceneItems.AnimationClasses
{
    public struct AnimationFrame
    {
        private int _duration;
        private String _area;

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public String Area
        {
            get { return _area; }
            set { _area = value; }
        }

        public AnimationFrame(int duration, String area)
        {
            this._duration = duration;
            this._area = area;
        }
    }
}
