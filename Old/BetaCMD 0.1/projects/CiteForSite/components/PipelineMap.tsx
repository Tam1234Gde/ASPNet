'use client'

import { useRef, useEffect } from 'react'
import { motion } from 'framer-motion'

const pipeline = [
  {
    title: 'DISCOVERY & ARCHITECTURE',
    subtitle: 'ИССЛЕДОВАНИЕ',
    description: 'Modular modeling',
    icon: '◉',
  },
  {
    title: 'DESIGN & PROTOTYPING',
    subtitle: 'ДИЗАЙН',
    description: 'Interactive wireframes & visual systems',
    icon: '◈',
  },
  {
    title: 'DEVELOPMENT',
    subtitle: 'РАЗРАБОТКА',
    description: 'Fullstack code integration',
    icon: '◬',
  },
  {
    title: 'QA & TESTING',
    subtitle: 'ТЕСТИРОВАНИЕ',
    description: 'Automated test suites & fault tolerance',
    icon: '◆',
  },
  {
    title: 'LAUNCH & SUPPORT',
    subtitle: 'ЗАПУСК',
    description: 'Deployment to staging & production cloud',
    icon: '◉',
  },
]

export default function PipelineMap() {
  return (
    <section
      id="process"
      className="relative min-h-screen w-full bg-dark-deeper py-32"
      style={{ perspective: '1500px' }}
    >
      <div className="max-w-[1600px] mx-auto px-8">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          className="mb-16"
        >
          <h2 className="text-6xl font-bold mb-4">
            DEVELOPMENT <span className="text-neon-green italic">PIPELINE</span>
          </h2>
          <p className="text-xl text-gray-400">Discovery to Launch</p>
        </motion.div>

        <div
          className="relative"
          style={{
            transform: 'rotateX(30deg) rotateY(-10deg)',
            transformStyle: 'preserve-3d',
          }}
        >
          <div className="relative flex flex-col gap-16">
            {pipeline.map((stage, index) => (
              <motion.div
                key={index}
                initial={{ opacity: 0, x: -50, z: -100 }}
                whileInView={{ opacity: 1, x: 0, z: 0 }}
                transition={{ duration: 0.6, delay: index * 0.15 }}
                className="relative"
                style={{ transformStyle: 'preserve-3d' }}
              >
                <div className="flex items-center gap-8">
                  <div className="glass-panel-strong w-[600px] p-8 relative group hover:scale-105 transition-transform">
                    <div className="text-5xl text-neon-green mb-4">{stage.icon}</div>
                    <div className="text-sm font-mono text-neon-green mb-2">{stage.subtitle}</div>
                    <h3 className="text-2xl font-bold mb-3">{stage.title}</h3>
                    <p className="text-gray-400 text-sm">{stage.description}</p>

                    <div className="absolute top-8 right-8 w-12 h-12 border border-neon-green/30 rounded-lg opacity-0 group-hover:opacity-100 transition-opacity" />
                  </div>

                  <div className="relative flex-1 h-1">
                    <motion.div
                      initial={{ scaleX: 0 }}
                      whileInView={{ scaleX: 1 }}
                      transition={{ duration: 0.8, delay: index * 0.15 + 0.3 }}
                      className="h-full bg-gradient-to-r from-neon-green to-neon-green/30 origin-left"
                    />
                    <motion.div
                      initial={{ scale: 0 }}
                      whileInView={{ scale: 1 }}
                      transition={{ duration: 0.4, delay: index * 0.15 + 0.6 }}
                      className="absolute right-0 top-1/2 -translate-y-1/2 w-3 h-3 bg-neon-green rounded-full shadow-lg shadow-neon-green/50"
                    />
                  </div>
                </div>

                {index < pipeline.length - 1 && (
                  <motion.svg
                    initial={{ pathLength: 0, opacity: 0 }}
                    whileInView={{ pathLength: 1, opacity: 0.5 }}
                    transition={{ duration: 1, delay: index * 0.15 + 0.4 }}
                    className="absolute left-[300px] top-full h-16 w-1"
                    style={{ overflow: 'visible' }}
                  >
                    <motion.path
                      d="M 0 0 L 0 64"
                      stroke="#00FF66"
                      strokeWidth="2"
                      fill="none"
                      strokeDasharray="4 4"
                    />
                  </motion.svg>
                )}
              </motion.div>
            ))}
          </div>
        </div>
      </div>
    </section>
  )
}
