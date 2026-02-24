import { useEffect, useState } from 'react'

const initialForm = {
  nombre: '',
  edad: 0,
  fechaNacimiento: '',
  salario: 0,
}

export default function ClienteForm({ onSubmit, selectedCliente, onCancel, saving }) {
  const [form, setForm] = useState(initialForm)
  const [errors, setErrors] = useState({})

  useEffect(() => {
    if (selectedCliente) {
      setForm({
        nombre: selectedCliente.nombre,
        edad: selectedCliente.edad,
        fechaNacimiento: selectedCliente.fechaNacimiento?.split('T')[0] ?? '',
        salario: selectedCliente.salario,
      })
      setErrors({})
    } else {
      setForm(initialForm)
      setErrors({})
    }
  }, [selectedCliente])

  const validate = () => {
    const nextErrors = {}

    if (!form.nombre || form.nombre.trim().length < 2 || form.nombre.trim().length > 120) {
      nextErrors.nombre = 'El nombre es requerido y debe tener entre 2 y 120 caracteres.'
    }

    if (Number(form.edad) < 0 || Number(form.edad) > 120) {
      nextErrors.edad = 'La edad debe estar entre 0 y 120.'
    }

    if (!form.fechaNacimiento) {
      nextErrors.fechaNacimiento = 'La fecha de nacimiento es requerida.'
    } else if (new Date(form.fechaNacimiento) > new Date()) {
      nextErrors.fechaNacimiento = 'La fecha de nacimiento no puede ser futura.'
    }

    if (Number(form.salario) < 0) {
      nextErrors.salario = 'El salario debe ser mayor o igual a 0.'
    }

    setErrors(nextErrors)
    return Object.keys(nextErrors).length === 0
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (!validate()) return

    await onSubmit({
      ...form,
      nombre: form.nombre.trim(),
      edad: Number(form.edad),
      salario: Number(form.salario),
      fechaNacimiento: new Date(form.fechaNacimiento).toISOString().slice(0, 10),
    })

    if (!selectedCliente) {
      setForm(initialForm)
    }
  }

  return (
    <form onSubmit={handleSubmit}>
      <h2>{selectedCliente ? 'Editar cliente' : 'Nuevo cliente'}</h2>

      <label>
        Nombre
        <input
          type="text"
          value={form.nombre}
          onChange={(e) => setForm((prev) => ({ ...prev, nombre: e.target.value }))}
        />
        {errors.nombre && <span className="error">{errors.nombre}</span>}
      </label>

      <label>
        Edad
        <input
          type="number"
          min="0"
          max="120"
          value={form.edad}
          onChange={(e) => setForm((prev) => ({ ...prev, edad: e.target.value }))}
        />
        {errors.edad && <span className="error">{errors.edad}</span>}
      </label>

      <label>
        Fecha de nacimiento
        <input
          type="date"
          value={form.fechaNacimiento}
          onChange={(e) => setForm((prev) => ({ ...prev, fechaNacimiento: e.target.value }))}
        />
        {errors.fechaNacimiento && <span className="error">{errors.fechaNacimiento}</span>}
      </label>

      <label>
        Salario
        <input
          type="number"
          step="0.01"
          min="0"
          value={form.salario}
          onChange={(e) => setForm((prev) => ({ ...prev, salario: e.target.value }))}
        />
        {errors.salario && <span className="error">{errors.salario}</span>}
      </label>

      <div className="actions">
        <button type="submit" disabled={saving}>{saving ? 'Guardando...' : 'Guardar'}</button>
        {selectedCliente && (
          <button type="button" onClick={onCancel}>Cancelar edición</button>
        )}
      </div>
    </form>
  )
}
