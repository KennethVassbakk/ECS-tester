using System.Collections.Generic;
using UnityEngine;

namespace DFLite.Helpers
{
    /// <summary>
    /// Represents a rectangle
    /// from bottom left to top right.
    /// </summary>
    ///      ______.max
    ///     |      |
    ///     |      |
    ///     .______|
    ///   min
    [System.Serializable]
    public struct RectI
    {
        public Vector2Int min;
        public Vector2Int max;

        public int width => this.max.x - this.min.x;
        public int height => this.max.y - this.min.y;
        public int area => this.width * this.height;
        public Vector2Int size => new Vector2Int(this.width, this.height);

        /// <summary>
        /// Create from min, max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public RectI(Vector2Int min, Vector2Int max) {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Create from min position, using width & height
        /// </summary>
        /// <param name="min"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectI(Vector2Int min, int width, int height) {
            this.min = min;
            this.max = new Vector2Int(this.min.x + width, this.min.y + height);
        }

        /// <summary>
        /// Enumerate over rects
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Vector2Int> GetEnumerator() {
            for (var x = this.min.x; x < this.max.x; x++) {
                for (var y = this.min.y; y < this.max.y; y++) {
                    yield return new Vector2Int(x, y);
                }
            }
        }

        /// <summary>
        /// Clip this RectI inside another RectI
        /// </summary>
        /// <param name="other"></param>
        public void Clip(RectI other) {
            if (this.min.x < other.min.x) {
                this.min.x = other.min.x;
            }

            if (this.max.x > other.max.x) {
                this.max.x = other.max.x;
            }

            if (this.min.y < other.min.y) {
                this.min.y = other.min.y;
            }

            if (this.max.y < other.max.y) {
                this.max.y = other.max.y;
            }
        }

        /// <summary>
        /// Check if Vector2Int is inside our rectangle
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Contains(Vector2Int v) {
            return (
                v.x >= this.min.x &&
                v.y >= this.min.y &&
                v.x < this.max.x &&
                v.y < this.max.y
            );
        }

        /// <summary>
        /// Get the Rect HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return this.min.x + this.min.y * this.width + this.max.x * this.height + this.max.y;
        }

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return "RectI(min: " + this.min + ", max: " + this.max + ", size: " + this.size + ", area: " + this.area +
                   ")";
        }
    }
}