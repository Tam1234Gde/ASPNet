# CORETECH-IT

High-end interactive website for custom web development studio built with Next.js 14, React, Three.js, GSAP, and Framer Motion.

## Tech Stack

- **Framework**: Next.js 14 (App Router)
- **Styling**: Tailwind CSS
- **3D Graphics**: Three.js, React Three Fiber, Drei
- **Animations**: Framer Motion, GSAP with ScrollTrigger
- **Smooth Scrolling**: Lenis
- **Language**: TypeScript

## Getting Started

Install dependencies:

```bash
npm install
```

Run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) in your browser.

## Features

- **Hero Section**: Interactive 3D liquid chrome core with WebGL, floating glass widgets, magnetic cursor
- **Services Rail**: Horizontal scroll-driven carousel with glassmorphic cards
- **Cases Grid**: 3D isometric project showcase with hover interactions
- **Tech Terminal**: Live console with animated logs and real-time metrics
- **Pipeline Map**: 3D isometric development roadmap with connected nodes
- **Contact CTA**: Interactive quantum core with glass-panel contact form
- **Custom Cursor**: Magnetic trail effect with smooth momentum
- **Smooth Scroll**: Lenis integration for buttery-smooth scrolling

## Design System

- Background: Deep matte graphite (#0A0A0C, #050505)
- Accent: Neon matrix green (#00FF66)
- Materials: Glassmorphism with backdrop-blur
- Typography: Inter (body), DM Serif Display (headings), Monospace (code)

## Project Structure

```
├── app/
│   ├── layout.tsx
│   ├── page.tsx
│   └── globals.css
├── components/
│   ├── Hero.tsx
│   ├── ServicesRail.tsx
│   ├── CasesGrid.tsx
│   ├── TechTerminal.tsx
│   ├── PipelineMap.tsx
│   ├── ContactCTA.tsx
│   └── LiquidCore.tsx
└── lib/
    ├── SmoothScroll.tsx
    └── MagneticCursor.tsx
```

## Commands

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run start` - Start production server
- `npm run lint` - Run ESLint

## License

Private project for CORETECH-IT.
