using System;
using System.Diagnostics;

namespace Rox.Util {

    public class FPS {

        private Stopwatch _secondTimer;
        private int _frameCount = 0;

        public FPS() {
            
        }
        
        public void Start() {
            _secondTimer = Stopwatch.StartNew();
        }

        public void Tick() {
            if (_secondTimer.ElapsedMilliseconds > 1000) {
                OnFrameCount?.Invoke(_frameCount);

                _frameCount = 0;
                _secondTimer.Reset();
                _secondTimer.Start();
            }
        }

        public void Frame() {
            _frameCount++;
        }

        public event Action<int> OnFrameCount;
    }
}
