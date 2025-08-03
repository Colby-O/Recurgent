using System.Collections.Generic;
using PlazmaGames.Attribute;
using UnityEngine;

namespace Recursive.Player
{
    public class Recording
    {
		private List<Frame> _frames = new();

        public void Clear() => _frames.Clear();

        public void AddFrame(Frame f) => _frames.Add(f);

        public int FrameCount() => _frames.Count;
        public Frame GetFrame(int i) => _frames[i];

        public Recording Clone()
        {
            Recording n = new Recording();
            n._frames = new List<Frame>(_frames);
            return n;
        }
    }
}
