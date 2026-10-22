'use client'

import { useRef, useEffect } from 'react'
import { motion } from 'framer-motion'
import gsap from 'gsap'
import { ScrollTrigger } from 'gsap/ScrollTrigger'

if (typeof window !== 'undefined') {
  gsap.registerPlugin(ScrollTrigger)
}

const services = [
  {
    title: 'HIGHLOAD WEB SYSTEMS',
    description: 'Архитектура микросервисов, сложные визуализации, привлечение пользователей',
    icon: '⬡',
    code: 'compile.static = apilsprocess(bytes)',
  },
  {
    title: 'ИНДИВИДУАЛЬНЫЕ БЭКЕНД-ПЛАТФОРМЫ',
    description: 'Архитектура микросервисов, интеграции API, производительность и масштабируемость',
    icon: '◈',
    code: 'backend.optimize = microservices.scale()',
    featured: true,
  },
  {
    title: 'ИНТЕРАКТИВНЫЙ 3D WEBGL',
    description: 'Кинематографичные интерфейсы, сложные визуализации, привлечение пользователей',
    icon: '◬',
    code: 'render.webgl = scene.animate()',
  },
]

export default function ServicesRail() {
  const sectionRef = useRef<HTMLElement>(null)
  const railRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (!sectionRef.current || !railRef.current) return

    const cards = railRef.current.querySelectorAll('.service-card')
    const totalWidth = railRef.current.scrollWidth - window.innerWidth

    gsap.to(railRef.current, {
      x: -totalWidth,
      ease: 'none',
      scrollTrigger: {
        trigger: sectionRef.current,
        start: 'top top',
        end: '+=300%',
        scrub: 1,
        pin: true,
      },
    })

    return () => {
      ScrollTrigger.getAll().forEach(trigger => trigger.kill())
    }
  }, [])

  return (
    <section
      ref={sectionRef}
      id="services"
      className="relative h-screen w-full bg-dark-base overflow-hidden"
    >
      <div className="absolute inset-0 flex items-center">
        <div ref={railRef} className="flex gap-8 px-8">
          {services.map((service, index) => (
            <motion.div
              key={index}
              className={`service-card glass-panel-strong relative flex-shrink-0 w-[500px] h-[600px] p-8 ${
                service.featured ? 'neon-glow scale-105' : ''
              }`}
              initial={{ opacity: 0, y: 50 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: index * 0.1 }}
            >
              {service.featured && (
                <div className="absolute -top-3 left-8 px-4 py-1 bg-neon-green text-dark-base text-xs font-bold rounded-full">
                  ПОПУЛЯРНО
                </div>
              )}

              <div className="text-6xl mb-6 text-neon-green">{service.icon}</div>

              <h3 className="text-3xl font-bold mb-4 leading-tight">{service.title}</h3>

              <p className="text-gray-400 text-sm leading-relaxed mb-8">{service.description}</p>

              <div className="absolute bottom-8 left-8 right-8">
                <div className="glass-panel p-4 font-mono text-xs text-neon-green">
                  {service.code}
                </div>
              </div>

              <div className="absolute top-8 right-8 w-16 h-16 border border-neon-green/30 rounded-lg" />
            </motion.div>
          ))}
        </div>
      </div>

      <div className="absolute top-8 left-8 z-10">
        <h2 className="text-5xl font-bold">
          НАШИ <span className="text-neon-green italic">УСЛУГИ</span>
        </h2>
      </div>
    </section>
  )
}
