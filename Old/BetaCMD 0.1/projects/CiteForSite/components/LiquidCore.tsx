'use client'

import { useRef, useEffect } from 'react'
import { useFrame } from '@react-three/fiber'
import { MeshDistortMaterial, useTexture } from '@react-three/drei'
import * as THREE from 'three'

export default function LiquidCore() {
  const meshRef = useRef<THREE.Mesh>(null)
  const materialRef = useRef<any>(null)

  useEffect(() => {
    if (meshRef.current) {
      meshRef.current.rotation.x = 0.3
      meshRef.current.rotation.y = 0.3
    }
  }, [])

  useFrame((state) => {
    if (meshRef.current) {
      meshRef.current.rotation.y += 0.002
      meshRef.current.rotation.x = Math.sin(state.clock.elapsedTime * 0.3) * 0.1
    }

    if (materialRef.current) {
      materialRef.current.distort = 0.4 + Math.sin(state.clock.elapsedTime) * 0.1
    }
  })

  return (
    <group>
      <mesh ref={meshRef} scale={2.5}>
        <torusKnotGeometry args={[1, 0.3, 256, 64]} />
        <MeshDistortMaterial
          ref={materialRef}
          color="#00FF66"
          emissive="#00FF66"
          emissiveIntensity={0.5}
          metalness={0.9}
          roughness={0.1}
          distort={0.4}
          speed={2}
          transparent
          opacity={0.9}
        />
      </mesh>
      
      <mesh scale={3}>
        <torusKnotGeometry args={[1, 0.15, 128, 32]} />
        <meshStandardMaterial
          color="#ffffff"
          metalness={1}
          roughness={0.1}
          transparent
          opacity={0.3}
        />
      </mesh>

      <pointLight position={[2, 2, 2]} intensity={1} color="#00FF66" />
      <pointLight position={[-2, -2, -2]} intensity={0.5} color="#00FF66" />
    </group>
  )
}
