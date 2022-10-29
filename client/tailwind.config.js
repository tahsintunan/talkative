/** @type {import('tailwindcss').Config} */
const themeColors = {
  primary: "var(--primary)",
  primaryDark: "var(--primary-dark)",
  primaryLight: "var(--primary-light)",
  secondary: "var(--secondary)",
  error: "var(--error)",
};

module.exports = {
  mode: "jit",
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: themeColors,
      backgroundColor: themeColors,
      borderColor: themeColors,
      ringColor: themeColors,
    },
  },
};
