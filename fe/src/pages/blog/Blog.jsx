import React, { useEffect, useState } from 'react';
import { getBlogList } from '@/api/blog'; 
import { linkImg } from '@/utils/common';

const Blog = () => {
    const [blogs, setBlogs] = useState([]);

    useEffect(() => {
        // Gọi API khi component được mount
        fetchBlogs();
    }, []);

    const fetchBlogs = async () => {
        try {
            const response = await getBlogList({ pageIndex: 1, isAsc: true, searchValue: '' });
            if (response.data) {
                // Giả sử API trả về mảng blogs trong response.data
                setBlogs(response.data);
            }
        } catch (error) {
            console.error('Error fetching blogs:', error);
        }
    };

    return (
        <>
           <section className="home-slider js-fullheight owl-carousel">
      <div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_1.jpg')})` }}>
      	<div className="overlay"></div>
        <div className="container">
          <div className="row slider-text justify-content-center align-items-center">

            <div className="col-md-7 col-sm-12 text-center ftco-animate">
            	<h1 className="mb-3 mt-5 bread">Blog</h1>
            </div>

          </div>
        </div>
      </div>
    </section>
            
            <section className="ftco-section">
                <div className="container">

                    <div className="row">
                        {blogs.map((blog, index) => (
                            <div key={index} className="col-md-4 ftco-animate">
                                <div className="blog-entry">
                                    <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg(blog.pictures)})` }}>
                                    </a>
                                    <div className="text d-flex py-4">
                                        <div className="meta mb-3">
                                            <div><a href="#">{blog.email}</a></div>
                                            <div><a href="#">{blog.content}</a></div>
                                        </div>
                                        <div className="desc pl-3">
                                            <h3 className="heading"><a href="#">{blog.title}</a></h3>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>
        </>
    );
};

export default Blog;