import { NextConfig } from 'next'

const nextConfig: NextConfig = {
  reactStrictMode: true,
  transpilePackages: ['three', 'gsap'],
}

export default nextConfig
