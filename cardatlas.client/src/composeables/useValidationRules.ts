export function useValidationRules () {
  const required = (textInput: string) => !!textInput || 'Required.'
  const minLength = (minLength: number) => (textInput: string) => textInput.length >= minLength || `Must be at least ${minLength} characters.`
  const equalTo = (target: string, errorMessage?: string) => (textInput: string) => textInput === target || errorMessage || false
  const validEmailFormat = (emailInput: string) => /.+@.+\..+/.test(emailInput) || 'Invalid email.'

  return {
    required,
    minLength,
    equalTo,
    validEmailFormat,
  }
}
