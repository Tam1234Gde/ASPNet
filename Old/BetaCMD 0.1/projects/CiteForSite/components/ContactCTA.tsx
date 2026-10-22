'use client'

import { useRef, useState } from 'react'
import { Canvas } from '@react-three/fiber'
import { motion } from 'framer-motion'
import { OrbitControls, Sphere } from '@react-three/drei'

function QuantumCore() {
  return (
    <group>
      <Sphere args={[1.5, 64, 64]}>
        <meshStandardMaterial
          color="#00FF66"
          emissive="#00FF66"
          emissiveIntensity={0.8}
          metalness={0.9}
          roughness={0.1}
          transparent
          opacity={0.8}
        />
      </Sphere>

      <Sphere args={[2, 64, 64]}>
        <meshStandardMaterial
          color="#00FF66"
          transparent
          opacity={0.1}
          wireframe
        />
      </Sphere>

      <pointLight position={[0, 0, 0]} intensity={2} color="#00FF66" />
    </group>
  )
}

export default function ContactCTA() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    brief: '',
    usage: '',
    purpose: '',
  })

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  return (
    <section className="relative min-h-screen w-full bg-dark-base py-32 flex items-center justify-center overflow-hidden">
      <div className="max-w-[1600px] w-full mx-auto px-8">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          className="text-center mb-16"
        >
          <h2 className="text-7xl md:text-8xl font-bold leading-tight">
            LET'S BUILD
            <br />
            SOMETHING <span className="text-neon-green italic text-glow">RELENTLESS</span>
          </h2>
        </motion.div>

        <div className="grid md:grid-cols-2 gap-12 items-center">
          <motion.div
            initial={{ opacity: 0, x: -30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            className="space-y-6"
          >
            <div className="glass-panel p-4">
              <label className="block text-sm font-mono text-gray-400 mb-2">Name</label>
              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                placeholder="Наме деталь"
                className="w-full bg-transparent border-b border-neon-green/30 py-2 text-white focus:outline-none focus:border-neon-green transition-colors"
              />
            </div>

            <div className="glass-panel p-4">
              <label className="block text-sm font-mono text-gray-400 mb-2">Email</label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="your@email.com"
                className="w-full bg-transparent border-b border-neon-green/30 py-2 text-white focus:outline-none focus:border-neon-green transition-colors"
              />
            </div>

            <div className="glass-panel p-4">
              <label className="block text-sm font-mono text-gray-400 mb-2">Project Brief</label>
              <textarea
                name="brief"
                value={formData.brief}
                onChange={handleChange}
                placeholder="Напоминание о проекте"
                rows={4}
                className="w-full bg-transparent border-b border-neon-green/30 py-2 text-white focus:outline-none focus:border-neon-green transition-colors resize-none"
              />
            </div>

            <div className="glass-panel p-4">
              <label className="block text-sm font-mono text-gray-400 mb-2">Использование</label>
              <input
                type="text"
                name="usage"
                value={formData.usage}
                onChange={handleChange}
                className="w-full bg-transparent border-b border-neon-green/30 py-2 text-white focus:outline-none focus:border-neon-green transition-colors"
              />
            </div>

            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              className="w-full py-4 bg-neon-green text-dark-base text-lg font-bold rounded-full neon-glow-strong hover:shadow-2xl transition-all"
            >
              START PROJECT
            </motion.button>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, scale: 0.8 }}
            whileInView={{ opacity: 1, scale: 1 }}
            transition={{ duration: 1, delay: 0.3 }}
            className="relative h-[600px]"
          >
            <Canvas camera={{ position: [0, 0, 5] }}>
              <ambientLight intensity={0.3} />
              <QuantumCore />
              <OrbitControls enableZoom={false} autoRotate autoRotateSpeed={2} />
            </Canvas>

            <div className="absolute inset-0 pointer-events-none">
              <motion.div
                animate={{ rotate: 360 }}
                transition={{ duration: 20, repeat: Infinity, ease: 'linear' }}
                className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[400px] h-[400px] border border-neon-green/20 rounded-full"
              />
              <motion.div
                animate={{ rotate: -360 }}
                transition={{ duration: 15, repeat: Infinity, ease: 'linear' }}
                className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[300px] h-[300px] border border-neon-green/30 rounded-full"
              />
            </div>
          </motion.div>
        </div>
      </div>
    </section>
  )
}
