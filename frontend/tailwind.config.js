/** @type {import('tailwindcss').Config} */
export default {
  darkMode: 'class',
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        spotify: {
          black: '#121212',
          card: '#181818',
          green: '#1DB954'
        }
      }
    }
  },
  plugins: []
}
