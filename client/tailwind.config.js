/** @type {import('tailwindcss').Config} */
module.exports = {
  mode: "jit",
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        primary: "var(--primary)",
        primaryDark: "var(--primary-dark)",
        secondary: "var(--secondary)",
        error: "var(--error)",
        errorDark: "var(--error-dark)",
      },
      backgroundColor: {
        primary: "var(--primary)",
        primaryDark: "var(--primary-dark)",
        secondary: "var(--secondary)",
        error: "var(--error)",
        errorDark: "var(--error-dark)",
      },
      borderColor: {
        primary: "var(--primary)",
        primaryDark: "var(--primary-dark)",
        secondary: "var(--secondary)",
        error: "var(--error)",
        errorDark: "var(--error-dark)",
      },
      ringColor: {
        primary: "var(--primary)",
        primaryDark: "var(--primary-dark)",
        secondary: "var(--secondary)",
        error: "var(--error)",
      },
    },
  },
};
