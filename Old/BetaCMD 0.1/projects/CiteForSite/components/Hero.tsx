'use client'

import { motion } from 'framer-motion'
import { Canvas } from '@react-three/fiber'
import { OrbitControls, PerspectiveCamera } from '@react-three/drei'
import { useEffect, useRef, useState } from 'react'
import LiquidCore from './LiquidCore'
import gsap from 'gsap'
import { ScrollTrigger } from 'gsap/ScrollTrigger'

if (typeof window !== 'undefined') {
  gsap.registerPlugin(ScrollTrigger)
}

export default function Hero() {
  const sectionRef = useRef<HTMLElement>(null)
  const coreContainerRef = useRef<HTMLDivElement>(null)
  const [mousePosition, setMousePosition] = useState({ x: 0, y: 0 })

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      const x = (e.clientX / window.innerWidth) * 2 - 1
      const y = -(e.clientY / window.innerHeight) * 2 + 1
      setMousePosition({ x, y })
    }

    window.addEventListener('mousemove', handleMouseMove)
    return () => window.removeEventListener('mousemove', handleMouseMove)
  }, [])

  useEffect(() => {
    if (!sectionRef.current) return

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: sectionRef.current,
        start: 'top top',
        end: '+=100%',
        scrub: 1,
        pin: true,
      },
    })

    tl.to('.hero-title', {
      opacity: 0,
      y: -100,
      scale: 0.8,
    })
      .to(
        '.hero-subtitle',
        {
          opacity: 0,
          y: -80,
        },
        '<'
      )
      .to(
        coreContainerRef.current,
        {
          scale: 1.5,
          rotateY: 180,
        },
        '<'
      )
      .to('.glass-widget', {
        opacity: 0,
        x: (index) => (index % 2 === 0 ? -200 : 200),
        y: -100,
        stagger: 0.1,
      })

    return () => {
      tl.kill()
    }
  }, [])

  return (
    <section
      ref={sectionRef}
      className="relative h-screen w-full bg-dark-deeper overflow-hidden"
    >
      <nav className="fixed top-0 left-0 right-0 z-50 backdrop-blur-md bg-dark-base/80 border-b border-glass-border">
        <div className="max-w-[1600px] mx-auto px-8 py-6 flex items-center justify-between">
          <motion.div
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.6 }}
            className="text-2xl font-bold tracking-tight"
          >
            CORETECH-IT
          </motion.div>

          <div className="flex items-center gap-12">
            <motion.div
              initial={{ opacity: 0, y: -10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: 0.1 }}
              className="hidden md:flex items-center gap-8 text-sm tracking-wider"
            >
              <a href="#services" className="hover:text-neon-green transition-colors">
                УСЛУГИ
              </a>
              <a href="#cases" className="hover:text-neon-green transition-colors">
                КЕЙСЫ
              </a>
              <a href="#process" className="hover:text-neon-green transition-colors">
                ПРОЦЕСС
              </a>
            </motion.div>

            <motion.button
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              transition={{ duration: 0.6, delay: 0.2 }}
              whileHover={{ scale: 1.05 }}
              className="px-6 py-3 bg-neon-green text-dark-base rounded-full font-medium text-sm tracking-wide hover:shadow-lg hover:shadow-neon-green/50 transition-all"
            >
              ПОЛУЧИТЬ НАЧАЛО
            </motion.button>
          </div>
        </div>
      </nav>

      <div className="absolute inset-0 flex items-center justify-center">
        <div className="relative w-full h-full max-w-[1600px] mx-auto px-8">
          <div className="absolute left-8 top-1/2 -translate-y-1/2 z-20 max-w-2xl">
            <motion.div
              initial={{ opacity: 0, y: 30 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.8, delay: 0.3 }}
              className="hero-title"
            >
              <h1 className="text-7xl md:text-8xl font-display font-bold leading-[0.9] tracking-tight">
                ПРОЕКТИРОВАНИЕ{' '}
                <span className="text-neon-green italic text-glow">ЦИФРОВЫХ</span>
                <br />
                РЕАЛЬНОСТЕЙ{' '}
                <span className="text-neon-green italic text-glow">ВЫСОКОГО</span>
                <br />
                УРОВНЯ
              </h1>
            </motion.div>

            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.8, delay: 0.5 }}
              className="hero-subtitle mt-8 space-y-4"
            >
              <p className="text-2xl text-gray-300">Индивидуальная веб-разработка</p>
              <p className="text-sm font-mono text-neon-green tracking-wider">
                WEBGL • ВЫСОКАЯ НАГРУЗКА • ПРИЛОЖЕНИЯ
              </p>
            </motion.div>
          </div>

          <motion.div
            ref={coreContainerRef}
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 1.2, delay: 0.4 }}
            className="absolute right-[10%] top-1/2 -translate-y-1/2 w-[600px] h-[600px]"
            style={{
              transform: `translate3d(${mousePosition.x * 20}px, ${mousePosition.y * 20}px, 0) translateY(-50%)`,
              transition: 'transform 0.3s ease-out',
            }}
          >
            <Canvas>
              <PerspectiveCamera makeDefault position={[0, 0, 5]} />
              <ambientLight intensity={0.3} />
              <LiquidCore />
            </Canvas>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, x: -30 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8, delay: 0.7 }}
            className="glass-widget glass-panel absolute left-8 bottom-32 w-80 p-6 z-20 preserve-3d"
            style={{
              transform: `rotateY(${mousePosition.x * 5}deg) rotateX(${-mousePosition.y * 5}deg)`,
              transition: 'transform 0.3s ease-out',
            }}
          >
            <div className="flex gap-2 mb-4">
              <button className="px-3 py-1 text-xs bg-neon-green/20 text-neon-green rounded-md">
                Услуги
              </button>
              <button className="px-3 py-1 text-xs text-gray-400 hover:text-white">
                Панели
              </button>
              <button className="px-3 py-1 text-xs text-gray-400 hover:text-white">
                Фрагменты кода
              </button>
            </div>
            <div className="text-3xl font-bold text-neon-green">317.8</div>
            <div className="text-xs text-gray-400 mt-1">brandeode</div>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, x: 30 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8, delay: 0.8 }}
            className="glass-widget glass-panel absolute right-8 top-32 w-80 p-6 z-20 preserve-3d"
            style={{
              transform: `rotateY(${mousePosition.x * -5}deg) rotateX(${-mousePosition.y * 5}deg)`,
              transition: 'transform 0.3s ease-out',
            }}
          >
            <div className="flex items-center justify-between mb-4">
              <span className="text-xs text-gray-400">Динамика рынка</span>
              <span className="text-neon-green text-lg font-bold">+25.449%</span>
            </div>
            <div className="h-20 border border-glass-border rounded flex items-end gap-1 p-2">
              {[40, 65, 45, 80, 55, 90, 70].map((height, i) => (
                <div
                  key={i}
                  className="flex-1 bg-neon-green/30 rounded-sm"
                  style={{ height: `${height}%` }}
                />
              ))}
            </div>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.9 }}
            className="glass-widget glass-panel absolute right-8 bottom-32 w-96 p-6 z-20 font-mono text-xs preserve-3d"
            style={{
              transform: `rotateY(${mousePosition.x * -5}deg) rotateX(${-mousePosition.y * 5}deg)`,
              transition: 'transform 0.3s ease-out',
            }}
          >
            <div className="text-neon-green mb-3">ГРАФИЧЕСКИЙ ДВИЖОК</div>
            <div className="text-gray-400 leading-relaxed">
              <span className="text-neon-green">функ</span> ПРОВЕРИТЬ_ДАННЫЕ {'{'} <br />
              &nbsp;&nbsp;<span className="text-neon-green">вывод</span> "Успешно";
              <br />
              {'}'}
            </div>
            <div className="mt-4 flex items-center gap-2">
              <div className="w-2 h-2 bg-neon-green rounded-full animate-pulse" />
              <span className="text-gray-500 text-[10px]">LIVE STATUS: ACTIVE</span>
            </div>
          </motion.div>
        </div>
      </div>
    </section>
  )
}
