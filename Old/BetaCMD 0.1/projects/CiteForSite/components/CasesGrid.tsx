'use client'

import { useRef, useEffect, useState } from 'react'
import { motion } from 'framer-motion'

const projects = [
  { title: 'HIGH-PERFORMANCE DASHBOARD', category: 'HIGHLOAD' },
  { title: 'MOBILE ANDROID DEVELOP', category: 'MOBILE' },
  { title: 'IOS ANDROID', category: 'MOBILE' },
  { title: '3D WEBGL', category: 'INTERACTIVE' },
  { title: 'HIGHLOAD WEB SYSTEMS', category: 'BACKEND' },
  { title: 'ИНДИВИДУАЛЬНЫЕ БЭКЕНД SYSTEMS', category: 'BACKEND' },
  { title: 'INTERACTIVE UI/UX', category: 'DESIGN' },
  { title: 'CLOUD INFRASTRUCTURE', category: 'DEVOPS' },
]

export default function CasesGrid() {
  const gridRef = useRef<HTMLDivElement>(null)
  const [mousePos, setMousePos] = useState({ x: 0, y: 0 })

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      setMousePos({ x: e.clientX, y: e.clientY })
    }

    window.addEventListener('mousemove', handleMouseMove)
    return () => window.removeEventListener('mousemove', handleMouseMove)
  }, [])

  return (
    <section
      id="cases"
      className="relative min-h-screen w-full bg-dark-deeper py-32 overflow-hidden"
      style={{
        perspective: '1000px',
      }}
    >
      <div className="max-w-[1600px] mx-auto px-8 mb-16">
        <h2 className="text-6xl font-bold">
          НАШИ <span className="text-neon-green italic">КЕЙСЫ</span>
        </h2>
      </div>

      <div
        ref={gridRef}
        className="relative grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6 px-8 max-w-[1600px] mx-auto"
        style={{
          transform: 'rotateX(25deg) rotateZ(-5deg)',
          transformStyle: 'preserve-3d',
        }}
      >
        {projects.map((project, index) => (
          <motion.div
            key={index}
            initial={{ opacity: 0, y: 50 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, delay: index * 0.05 }}
            whileHover={{
              scale: 1.05,
              z: 50,
              boxShadow: '0 0 40px rgba(0, 255, 102, 0.4)',
            }}
            className="glass-panel-strong aspect-[4/3] p-6 cursor-pointer group relative"
            style={{
              transformStyle: 'preserve-3d',
            }}
          >
            <div className="absolute inset-0 bg-gradient-to-br from-neon-green/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity rounded-2xl" />

            <div className="relative z-10 flex flex-col h-full justify-between">
              <div>
                <div className="text-xs font-mono text-neon-green mb-4">{project.category}</div>
                <h3 className="text-lg font-bold leading-tight">{project.title}</h3>
              </div>

              <div className="opacity-0 group-hover:opacity-100 transition-opacity">
                <button className="px-4 py-2 bg-neon-green text-dark-base text-sm font-bold rounded-lg">
                  СМОТРЕТЬ ПРОЕКТ
                </button>
              </div>
            </div>

            <div className="absolute top-4 right-4 w-8 h-8 border border-neon-green/30 rounded-md opacity-0 group-hover:opacity-100 transition-opacity" />
          </motion.div>
        ))}
      </div>
    </section>
  )
}
