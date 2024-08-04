import React, { Component } from 'react';
import axiosInstance from '../utils/axiosInstance';
import 'bootstrap/dist/css/bootstrap.min.css';

export class CoachServiceBooking extends Component {
  constructor(props) {
    super(props);
    this.state = {
      services: [],
      searchQuery: '',
      clientName: '',
      clientEmail: '',
      clientPhone: '',
      bookingDate: '',
      selectedServiceId: null,
      successMessage: '',
      selectedServiceDetails: null,
      showModal: false,
      filters: {
        duration: '',
        price: '',
        availability: ''
      },
      currentPage: 1,
      totalPages: 1
    };
    this.searchServices = this.searchServices.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleDetailClick = this.handleDetailClick.bind(this);
    this.handleCloseModal = this.handleCloseModal.bind(this);
    this.handleFilterChange = this.handleFilterChange.bind(this);
    this.fetchServices = this.fetchServices.bind(this);
    this.handleBooking = this.handleBooking.bind(this);
  }

  componentDidMount() {
    this.fetchServices();
  }

  async fetchServices(pageNumber = 1, pageSize = 5) {
    try {
      const response = await axiosInstance.get(`/CoachingServices?PageNumber=${pageNumber}&PageSize=${pageSize}`);
      this.setState({
        services: response.data.items,
        currentPage: pageNumber,
        totalPages: Math.ceil(response.data.totalItems / pageSize)
      });
    } catch (error) {
      console.error('Error fetching services:', error);
    }
  }

  searchServices(event) {
    const query = event.target.value.toLowerCase();
    this.setState({ searchQuery: query });
  }

  handleFilterChange(event) {
    const { name, value } = event.target;
    this.setState(prevState => ({
      filters: {
        ...prevState.filters,
        [name]: value
      }
    }));
  }

  handleInputChange(event) {
    const { name, value } = event.target;
    this.setState({
      [name]: value,
    });
  }

  async handleDetailClick(serviceId) {
    try {
      const response = await axiosInstance.get(`/CoachingServices/${serviceId}`);
      this.setState({
        selectedServiceId: serviceId,
        selectedServiceDetails: response.data,
        successMessage: '',
        showModal: true,
      });
    } catch (error) {
      console.error('Detail fetch error:', error.response?.data || error.message);
    }
  }

  handleCloseModal() {
    this.setState({ showModal: false });
  }

  async handleBooking() {
    try {
      const { selectedServiceId } = this.state;
      await axiosInstance.post(`/CoachingServices/book/${selectedServiceId}`, {
        coachingServiceId: selectedServiceId
      });
      this.setState({
        successMessage: 'Booking successful!',
        showModal: false,
      });
    } catch (error) {
      console.error('Booking error:', error.response?.data || error.message);
      this.setState({
        successMessage: 'Booking failed!',
      });
    }
  }

  render() {
    const { services, searchQuery, selectedServiceDetails, showModal, filters, currentPage, totalPages, successMessage } = this.state;
    const filteredServices = services.filter(service => {
      const query = searchQuery.toLowerCase();
      const { serviceName, description, duration, price, serviceAvailability } = service;
      return (
        service.serviceName.toLowerCase().includes(query) &&
        (filters.duration === '' ||
          (filters.duration === '1-3' && duration >= 1 && duration <= 3) ||
          (filters.duration === '4-6' && duration >= 4 && duration <= 6) ||
          (filters.duration === '7-10' && duration >= 7 && duration <= 10) ||
          (filters.duration === '>10' && duration > 10)) &&
        (filters.price === '' ||
          (filters.price === '<100' && price < 100) ||
          (filters.price === '100-500' && price >= 100 && price <= 500) ||
          (filters.price === '501-1000' && price >= 501 && price <= 1000) ||
          (filters.price === '>1000' && price > 1000)) &&
        (filters.availability === '' || (filters.availability === 'Available' ? serviceAvailability : !serviceAvailability))
      );
    });

    const renderPagination = () => {
      const pages = [];
      for (let i = 1; i <= totalPages; i++) {
        pages.push(
          <button
            key={i}
            onClick={() => this.fetchServices(i)}
            disabled={currentPage === i}
            className={`btn ${currentPage === i ? 'btn-primary' : 'btn-secondary'}`}
            style={{ margin: '0 5px' }}
          >
            {i}
          </button>
        );
      }
      return pages;
    };

    return (
      <div className="container mt-5">
        <h1 className="text-center" style={{ fontSize: '40px' }}><strong>Coach Service Detail</strong></h1>

        <div className="search-filter-container mb-4">
          <div className="input-group mb-3">
            <input
              type="text"
              id="searchService"
              className="form-control"
              style={{ height: '50px', border: '1px solid #ccc', marginTop: '20px', backgroundColor: 'white' }}
              placeholder="Search Services"
              value={searchQuery}
              onChange={this.searchServices}
            />
          </div>
          <div className="filters">
            <div className="row">
              <div className="col-md-2">
                <select
                  name="duration"
                  className="form-control"
                  value={filters.duration}
                  onChange={this.handleFilterChange}
                >
                  <option value="">Duration (weeks)</option>
                  <option value="1-3">1-3 weeks</option>
                  <option value="4-6">4-6 weeks</option>
                  <option value="7-10">7-10 weeks</option>
                  <option value=">10">>10 weeks</option>
                </select>
              </div>
              <div className="col-md-2">
                <select
                  name="price"
                  className="form-control"
                  value={filters.price}
                  onChange={this.handleFilterChange}
                >
                  <option value="">Price</option>
                  <option value="<100">Less than $100</option>
                  <option value="100-500">$100 - $500</option>
                  <option value="501-1000">$501 - $1000</option>
                  <option value=">1000">More than $1000</option>
                </select>
              </div>
              <div className="col-md-2">
                <select
                  name="availability"
                  className="form-control"
                  value={filters.availability}
                  onChange={this.handleFilterChange}
                >
                  <option value="">Availability</option>
                  <option value="Available">Available</option>
                  <option value="Not Available">Not Available</option>
                </select>
              </div>
            </div>
          </div>
        </div>

        <ul className="list-group" id="serviceList">
          {filteredServices.map(service => (
            <li key={service.id} className="list-group-item d-flex justify-content-between align-items-center" style={{ height: '110px' }}>
              <div>
                {service.id}.   <strong>Service Name: {service.serviceName}</strong> (Duration: {service.duration} Weeks, Price: ${service.price})
              </div>
              <button
                className="btn btn-primary"
                style={{ width: '140px' }}
                onClick={() => this.handleDetailClick(service.id)}
              >
                Booking
              </button>
            </li>
          ))}
        </ul>

        {/* Pagination Component */}
        <div className="pagination mt-4">
          <button onClick={() => this.fetchServices(1)} disabled={currentPage === 1}>&laquo;</button>
          <button onClick={() => this.fetchServices(currentPage - 1)} disabled={currentPage === 1}>‹</button>
          {renderPagination()}
          <button onClick={() => this.fetchServices(currentPage + 1)} disabled={currentPage === totalPages}>›</button>
          <button onClick={() => this.fetchServices(totalPages)} disabled={currentPage === totalPages}>&raquo;</button>
        </div>

        {/* Detail Modal */}
        {showModal && (
          <div className="modal fade show" tabIndex="-1" role="dialog" style={{ display: 'block' }} aria-labelledby="detailModalLabel" aria-hidden="true">
            <div className="modal-dialog" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title" id="detailModalLabel">Service Detail</h5>
                  <button type="button" className="close" aria-label="Close" onClick={this.handleCloseModal}>
                    <span aria-hidden="true">&times;</span>
                  </button>
                </div>
                <div className="modal-body">
                  {selectedServiceDetails ? (
                    <div>
                      <h3><strong>Service Name:</strong> {selectedServiceDetails.serviceName}</h3>
                      <h4><strong>Description</strong>: {selectedServiceDetails.description}</h4>
                      <p><strong>Duration:</strong> {selectedServiceDetails.duration} Weeks</p>
                      <p><strong>Price:</strong> ${selectedServiceDetails.price}</p>
                      <p><strong>Availability:</strong> {selectedServiceDetails.serviceAvailability ? 'Available' : 'Not Available'}</p>
                      <p><strong>Announcement:</strong> {selectedServiceDetails.availabilityAnnouncement}</p>
                      <p><strong>Created By:</strong> {selectedServiceDetails.createdByUserName}</p>
                      <p><strong>Last Modified By:</strong> {selectedServiceDetails.lastModifiedByUserName}</p>
                      {successMessage && <div className="alert alert-success">{successMessage}</div>}
                    </div>
                  ) : (
                    <div>Loading...</div>
                  )}
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" onClick={this.handleCloseModal}>Close</button>
                  <button type="button" className="btn btn-primary" onClick={this.handleBooking}>Book</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default CoachServiceBooking;
