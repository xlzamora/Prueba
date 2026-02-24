import { useEffect, useState } from 'react'
import ClienteForm from '../components/ClienteForm'
import ClientesTable from '../components/ClientesTable'
import {
  createCliente,
  deleteCliente,
  getClienteById,
  getClientes,
  updateCliente,
} from '../api/clientesApi'

export default function ClientesPage() {
  const [clientes, setClientes] = useState([])
  const [selectedCliente, setSelectedCliente] = useState(null)
  const [loading, setLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')

  const loadClientes = async () => {
    setLoading(true)
    setError('')
    try {
      const data = await getClientes()
      setClientes(data)
    } catch (err) {
      setError(err.response?.data?.message || 'No se pudo cargar la lista de clientes.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    loadClientes()
  }, [])

  const handleCreateOrUpdate = async (payload) => {
    setSaving(true)
    setError('')
    try {
      if (selectedCliente) {
        await updateCliente(selectedCliente.clienteId, payload)
      } else {
        await createCliente(payload)
      }
      setSelectedCliente(null)
      await loadClientes()
    } catch (err) {
      setError(err.response?.data?.message || 'No se pudo guardar el cliente.')
    } finally {
      setSaving(false)
    }
  }

  const handleEdit = async (id) => {
    setError('')
    try {
      const cliente = await getClienteById(id)
      setSelectedCliente(cliente)
    } catch (err) {
      setError(err.response?.data?.message || 'No se pudo cargar el cliente seleccionado.')
    }
  }

  const handleDelete = async (id) => {
    if (!window.confirm(`¿Seguro que deseas eliminar el cliente ${id}?`)) return

    setError('')
    try {
      await deleteCliente(id)
      if (selectedCliente?.clienteId === id) {
        setSelectedCliente(null)
      }
      await loadClientes()
    } catch (err) {
      setError(err.response?.data?.message || 'No se pudo eliminar el cliente.')
    }
  }

  return (
    <main className="container">
      <h1>CRUD de Clientes</h1>
      {error && <div className="error-box">{error}</div>}

      <ClienteForm
        onSubmit={handleCreateOrUpdate}
        selectedCliente={selectedCliente}
        onCancel={() => setSelectedCliente(null)}
        saving={saving}
      />

      <ClientesTable
        clientes={clientes}
        loading={loading}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />
    </main>
  )
}
