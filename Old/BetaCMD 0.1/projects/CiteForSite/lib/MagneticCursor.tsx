'use client'

import { useEffect, useRef } from 'react'
import { motion } from 'framer-motion'

export default function MagneticCursor() {
  const cursorRef = useRef<HTMLDivElement>(null)
  const cursorDotRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    const cursor = cursorRef.current
    const cursorDot = cursorDotRef.current
    if (!cursor || !cursorDot) return

    let mouseX = 0
    let mouseY = 0
    let cursorX = 0
    let cursorY = 0
    let dotX = 0
    let dotY = 0

    const handleMouseMove = (e: MouseEvent) => {
      mouseX = e.clientX
      mouseY = e.clientY
    }

    const animate = () => {
      const dx = mouseX - cursorX
      const dy = mouseY - cursorY
      cursorX += dx * 0.1
      cursorY += dy * 0.1

      const dotDx = mouseX - dotX
      const dotDy = mouseY - dotY
      dotX += dotDx * 0.15
      dotY += dotDy * 0.15

      cursor.style.transform = `translate3d(${cursorX}px, ${cursorY}px, 0)`
      cursorDot.style.transform = `translate3d(${dotX}px, ${dotY}px, 0)`

      requestAnimationFrame(animate)
    }

    window.addEventListener('mousemove', handleMouseMove)
    animate()

    return () => {
      window.removeEventListener('mousemove', handleMouseMove)
    }
  }, [])

  return (
    <>
      <div
        ref={cursorRef}
        className="fixed top-0 left-0 w-8 h-8 pointer-events-none z-[9999] mix-blend-difference"
        style={{
          transform: 'translate3d(0, 0, 0)',
        }}
      >
        <div className="w-full h-full rounded-full border border-neon-green opacity-50" />
      </div>
      <div
        ref={cursorDotRef}
        className="fixed top-0 left-0 w-1.5 h-1.5 pointer-events-none z-[9999] -ml-[3px] -mt-[3px]"
        style={{
          transform: 'translate3d(0, 0, 0)',
        }}
      >
        <div className="w-full h-full rounded-full bg-neon-green" />
      </div>
    </>
  )
}
