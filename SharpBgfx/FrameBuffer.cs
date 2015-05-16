﻿using System;

namespace SharpBgfx {
    /// <summary>
    /// An aggregated frame buffer, with one or more attached texture surfaces.
    /// </summary>
    public unsafe struct FrameBuffer : IDisposable, IEquatable<FrameBuffer> {
        internal readonly ushort handle;

        /// <summary>
        /// Represents an invalid handle.
        /// </summary>
        public static readonly FrameBuffer Invalid = new FrameBuffer();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBuffer"/> struct.
        /// </summary>
        /// <param name="width">The width of the render target.</param>
        /// <param name="height">The height of the render target.</param>
        /// <param name="format">The format of the new surface.</param>
        /// <param name="flags">Texture sampling flags.</param>
        public FrameBuffer (int width, int height, TextureFormat format, TextureFlags flags = TextureFlags.ClampU | TextureFlags.ClampV) {
            handle = NativeMethods.bgfx_create_frame_buffer((ushort)width, (ushort)height, format, flags);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBuffer"/> struct.
        /// </summary>
        /// <param name="ratio">The amount to scale when the backbuffer resizes.</param>
        /// <param name="format">The format of the new surface.</param>
        /// <param name="flags">Texture sampling flags.</param>
        public FrameBuffer (BackbufferRatio ratio, TextureFormat format, TextureFlags flags = TextureFlags.ClampU | TextureFlags.ClampV) {
            handle = NativeMethods.bgfx_create_frame_buffer_scaled(ratio, format, flags);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBuffer"/> struct.
        /// </summary>
        /// <param name="attachments">A set of attachments from which to build the frame buffer.</param>
        /// <param name="destroyTextures">if set to <c>true</c>, attached textures will be destroyed when the frame buffer is destroyed.</param>
        public FrameBuffer (Texture[] attachments, bool destroyTextures = false) {
            var count = (byte)attachments.Length;
            var handles = stackalloc ushort[count];
            for (int i = 0; i < count; i++)
                handles[i] = attachments[i].handle;

            handle = NativeMethods.bgfx_create_frame_buffer_from_handles(count, handles, destroyTextures);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBuffer"/> struct.
        /// </summary>
        /// <param name="windowHandle">The OS window handle to which the frame buffer is attached.</param>
        /// <param name="width">The width of the render target.</param>
        /// <param name="height">The height of the render target.</param>
        /// <param name="depthFormat">A desired format for a depth buffer, if applicable.</param>
        public FrameBuffer (IntPtr windowHandle, int width, int height, TextureFormat depthFormat = TextureFormat.UnknownDepth) {
            handle = NativeMethods.bgfx_create_frame_buffer_from_nwh(windowHandle, (ushort)width, (ushort)height, depthFormat);
        }

        /// <summary>
        /// Releases the frame buffer.
        /// </summary>
        public void Dispose () {
            NativeMethods.bgfx_destroy_frame_buffer(handle);
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals (FrameBuffer other) {
            return handle == other.handle;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals (object obj) {
            var other = obj as FrameBuffer?;
            if (other == null)
                return false;

            return Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode () {
            return handle.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString () {
            return string.Format("Handle: {0}", handle);
        }

        /// <summary>
        /// Implements the equality operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>
        /// <c>true</c> if the two objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(FrameBuffer left, FrameBuffer right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the inequality operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>
        /// <c>true</c> if the two objects are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(FrameBuffer left, FrameBuffer right) {
            return !left.Equals(right);
        }
    }
}
