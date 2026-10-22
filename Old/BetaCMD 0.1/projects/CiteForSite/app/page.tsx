'use client'

import Hero from '@/components/Hero'
import ServicesRail from '@/components/ServicesRail'
import CasesGrid from '@/components/CasesGrid'
import TechTerminal from '@/components/TechTerminal'
import PipelineMap from '@/components/PipelineMap'
import ContactCTA from '@/components/ContactCTA'

export default function Home() {
  return (
    <main className="relative">
      <Hero />
      <ServicesRail />
      <CasesGrid />
      <TechTerminal />
      <PipelineMap />
      <ContactCTA />
    </main>
  )
}
