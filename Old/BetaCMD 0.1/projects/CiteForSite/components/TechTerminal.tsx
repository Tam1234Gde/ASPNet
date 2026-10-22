'use client'

import { useRef, useEffect, useState } from 'react'
import { motion } from 'framer-motion'

const techStack = ['.NET', 'React', 'Docker', 'WebGL', 'TypeScript', 'Azure']

const logs = [
  'Running compilation ons...',
  "Running: compiling object builders.NETS from 'sits'",
  'compiling code.depoatenty',
  'Build successful: 0 errors, 0 warnings',
  'Optimizing bundle size...',
  'Deploying to staging environment...',
]

export default function TechTerminal() {
  const [activeStack, setActiveStack] = useState<string | null>(null)
  const [visibleLogs, setVisibleLogs] = useState<string[]>([])
  const [cpuData, setCpuData] = useState<number[]>([40, 65, 45, 80, 55, 90, 70, 60])

  useEffect(() => {
    let index = 0
    const interval = setInterval(() => {
      if (index < logs.length) {
        setVisibleLogs((prev) => [...prev, logs[index]])
        index++
      } else {
        clearInterval(interval)
      }
    }, 800)

    return () => clearInterval(interval)
  }, [])

  useEffect(() => {
    const interval = setInterval(() => {
      setCpuData((prev) => [...prev.slice(1), Math.random() * 100])
    }, 1000)

    return () => clearInterval(interval)
  }, [])

  return (
    <section className="relative min-h-screen w-full bg-dark-base py-32 flex items-center justify-center">
      <div className="max-w-[1200px] w-full mx-auto px-8">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          className="glass-panel-strong p-8 neon-glow"
        >
          <div className="flex items-center justify-between mb-8">
            <h2 className="text-3xl font-bold font-mono">TECH STACK CONSOLE</h2>
            <div className="flex gap-2">
              <div className="w-3 h-3 rounded-full bg-red-500" />
              <div className="w-3 h-3 rounded-full bg-yellow-500" />
              <div className="w-3 h-3 rounded-full bg-green-500" />
            </div>
          </div>

          <div className="flex flex-wrap gap-3 mb-8">
            {techStack.map((tech, index) => (
              <motion.button
                key={tech}
                initial={{ opacity: 0, scale: 0.9 }}
                whileInView={{ opacity: 1, scale: 1 }}
                transition={{ duration: 0.4, delay: index * 0.1 }}
                whileHover={{ scale: 1.05 }}
                onClick={() => setActiveStack(tech === activeStack ? null : tech)}
                className={`px-6 py-2 rounded-full font-mono text-sm border transition-all ${
                  activeStack === tech
                    ? 'bg-neon-green text-dark-base border-neon-green'
                    : 'border-neon-green/30 text-neon-green hover:border-neon-green'
                }`}
              >
                {tech}
              </motion.button>
            ))}
          </div>

          <div className="glass-panel bg-dark-deeper/50 p-6 rounded-xl mb-6 min-h-[300px] font-mono text-sm">
            {visibleLogs.map((log, index) => (
              <motion.div
                key={index}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.3 }}
                className={`mb-2 ${
                  log.includes('successful') || log.includes('Deploying')
                    ? 'text-neon-green'
                    : 'text-gray-400'
                }`}
              >
                <span className="text-neon-green mr-3">{'>'}</span>
                {log}
              </motion.div>
            ))}
            <motion.div
              animate={{ opacity: [1, 0] }}
              transition={{ duration: 0.8, repeat: Infinity }}
              className="inline-block w-2 h-4 bg-neon-green ml-1"
            />
          </div>

          <div className="flex items-end justify-between">
            <div className="glass-panel p-4 rounded-lg flex-1 mr-6">
              <div className="text-xs text-gray-400 mb-2">CPU / RAM USAGE</div>
              <div className="flex items-end gap-1 h-20">
                {cpuData.map((value, index) => (
                  <motion.div
                    key={index}
                    initial={{ height: 0 }}
                    animate={{ height: `${value}%` }}
                    transition={{ duration: 0.3 }}
                    className="flex-1 bg-neon-green/50 rounded-sm"
                  />
                ))}
              </div>
            </div>

            <div className="glass-panel px-6 py-3 rounded-lg font-mono text-xs">
              <span className="text-neon-green">STATUS: SUCCESS</span>
              <span className="text-gray-400"> | DEPLOYING TO STAGING</span>
            </div>
          </div>
        </motion.div>
      </div>
    </section>
  )
}
