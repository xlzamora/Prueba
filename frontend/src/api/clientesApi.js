import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5099/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

export const getClientes = async () => {
  const { data } = await api.get('/clientes')
  return data
}

export const getClienteById = async (id) => {
  const { data } = await api.get(`/clientes/${id}`)
  return data
}

export const createCliente = async (payload) => {
  const { data } = await api.post('/clientes', payload)
  return data
}

export const updateCliente = async (id, payload) => {
  const { data } = await api.put(`/clientes/${id}`, payload)
  return data
}

export const deleteCliente = async (id) => {
  await api.delete(`/clientes/${id}`)
}
