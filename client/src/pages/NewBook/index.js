import React, { useState, useEffect } from 'react';
import {Link, useNavigate, useParams} from 'react-router-dom'; 
import { FiArrowLeft } from 'react-icons/fi';
import api from '../../services/api';
import './styles.css';
import logoImage from '../../assets/logo.svg';

export default function NewBook() {

	const [id, setId] = useState(null);
	const [author, setAuthor] = useState('');
	const [title, setTitle] = useState('');
	const [launchDate, setLaunchDate] = useState('');
	const [price, setPrice] = useState('');

	const { bookId } = useParams();

	const navigate = useNavigate();

	const accessToken = localStorage.getItem('accessToken');

	const authorization = {
		headers: {
			Authorization: `Bearer ${accessToken}`
		}
	}

	useEffect(() => {
		if (bookId === '0') return; 
		else loadBook();
	}, [bookId]);


	async function loadBook() {
		try {
			const response = await api.get(`api/book/v1/${bookId}`, authorization);
			let adjustedDate = response.data.launchDate.split("T", 10)[0];

			setId(response.data.id);
			setTitle(response.data.title);
			setAuthor(response.data.author);
			setPrice(response.data.price);
			setLaunchDate(adjustedDate);
		} catch (error) {
			alert('Error recovering book data, please try again.');
			navigate('/books');
		}
	}

	async function saveOrUpdate(e) {
			e.preventDefault();

		const data = {
			title,
			author,
			launchDate,
			price,
		}
		try {
			if (bookId === '0') {
				await api.post('api/book/v1', data, {
					headers: {
						Authorization: `Bearer ${accessToken}`
					}
				});
			} else {
				data.id = id;
				await api.put('api/book/v1', data, {
					headers: {
						Authorization: `Bearer ${accessToken}`
					}
				});
			}
		} catch (error) {
			alert('Error adding book, please try again.');
		}
		navigate('/books');
	}

  return (
	<div className="new-book-container">
    	<div className="content">
			<section className="form">
				<img src={logoImage} alt="Erudio" />
				<h1>{bookId === '0' ? 'Add New Book' : 'Update Book'}</h1>
				<p>Enter the book information and click on {bookId === '0' ? "'Add'" : "'Update'"}! ID: {bookId}</p>
				<Link className="back-link" to="/books">
					<FiArrowLeft size={16} color="#251FC5" />
					Back to Books
				</Link>
			</section>
			<form onSubmit={saveOrUpdate}>
				<input 
					value={title}
					onChange={e => setTitle(e.target.value)}
					placeholder="Title" 
				/>
				<input 
					value={author}
					onChange={e => setAuthor(e.target.value)}
					placeholder="Author" 
				/>
				<input 
					value={launchDate}
					onChange={e => setLaunchDate(e.target.value)}
					type="date" 
				/>
				<input 
					value={price}
					onChange={e => setPrice(e.target.value)}
					placeholder="Price" 
				/>
				<button className="button" type="submit">{bookId === '0' ? 'Add' : 'Update'}</button>
			</form>
      	</div>
    </div>
  );
}