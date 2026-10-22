/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './pages/**/*.{js,ts,jsx,tsx,mdx}',
    './components/**/*.{js,ts,jsx,tsx,mdx}',
    './app/**/*.{js,ts,jsx,tsx,mdx}',
  ],
  theme: {
    extend: {
      colors: {
        'dark-base': '#0A0A0C',
        'dark-deeper': '#050505',
        'neon-green': '#00FF66',
        'glass-surface': 'rgba(255, 255, 255, 0.03)',
        'glass-border': 'rgba(0, 255, 102, 0.13)',
      },
      fontFamily: {
        sans: ['var(--font-inter)', 'system-ui', 'sans-serif'],
        display: ['var(--font-dm-serif)', 'serif'],
        mono: ['var(--font-mono)', 'monospace'],
      },
      backdropBlur: {
        'glass': '16px',
        'glass-strong': '24px',
      },
    },
  },
  plugins: [],
}
