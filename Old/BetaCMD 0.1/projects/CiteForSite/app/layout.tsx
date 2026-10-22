'use client'

import './globals.css'
import { Inter, DM_Serif_Display } from 'next/font/google'
import SmoothScroll from '@/lib/SmoothScroll'
import MagneticCursor from '@/lib/MagneticCursor'

const inter = Inter({ 
  subsets: ['latin', 'cyrillic'],
  variable: '--font-inter',
})

const dmSerif = DM_Serif_Display({ 
  weight: ['400'],
  subsets: ['latin'],
  variable: '--font-dm-serif',
})

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="ru" className={`${inter.variable} ${dmSerif.variable}`}>
      <body className={inter.className}>
        <SmoothScroll />
        <MagneticCursor />
        {children}
      </body>
    </html>
  )
}
