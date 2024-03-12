import React, { useEffect } from 'react';
import { linkImg } from '@/utils/common';
import { useAppDispatch, useAppSelector } from '../../store';
import { actionGetBlogList } from '../../store/blog/action';

export const Blog = () => {
    const dispatch = useAppDispatch();
    const blogs = useAppSelector(({ blogs }) => blogs.blogs);
    console.log('blogs::', blogs)
    useEffect(() => {
        dispatch(actionGetBlogList({
            pageIndex: 1,
            isAsc: true,
            searchValue: ''
        }))
    }, []);


    return (
        <>
            <section className="home-slider js-fullheight owl-carousel">
                <div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_1.jpg')})` }}>
                    <div className="overlay"></div>
                    <div className="container">
                        <div className="row no-gutters slider-text js-fullheight align-items-center justify-content-end" data-scrollax-parent="true">
                            <div className="col-md-7 text ftco-animate" data-scrollax=" properties: { translateY: '70%' }">
                                <h1 className="mb-3 mt-5 bread">Blog</h1>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section className="ftco-section">
                <div className="container">
                    <div className="row justify-content-center mb-5 pb-3">
                        <div className="col-md-7 heading-section ftco-animate">
                            <h2 className="mb-4">Recent Blog</h2>
                            <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in</p>
                        </div>

                    </div>

                    <div className="row">
                        {blogs?.map((item) => (
                            <div>
                                <div key={item.blogId} className="col-md-4 ftco-animate fadeInUp ftco-animated">
                                    <a href="blog-single.html" className="block-20" style={{
                                        backgroundImage: `url(data:image/jpeg;base64,${item.pictures})`,
                                        backgroundSize: 'cover',
                                        backgroundPosition: 'center',
                                        backgroundRepeat: 'no-repeat',
                                        height: '200px',
                                        width: '300%', 
                                    }}></a>
                                    <div className="blog-entry">
                                        <div className="text d-flex py-4">
                                            <div className="meta">

                                                <h3 className="heading"><a href="#">{item.title}</a></h3>
                                                <div><a href="#">{item.content}</a></div>
                                            </div>
                                            <div className="desc pl-3">
                                                <div><a href="#">{item.email}</a></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                    <div className="row">
                        <div className="col-md-4 ftco-animate">
                            <div className="blog-entry">
                                <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_2.jpg')})` }}>
                                </a>
                                <div className="text d-flex py-4">
                                    <div className="meta mb-3">
                                        <div><a href="#">Sep. 20, 2018</a></div>
                                        <div><a href="#">Admin</a></div>
                                        <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                                    </div>
                                    <div className="desc pl-3">
                                        <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-4 ftco-animate">
                            <div className="blog-entry" data-aos-delay="100">
                                <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_2.jpg')})` }}>
                                </a>
                                <div className="text d-flex py-4">
                                    <div className="meta mb-3">
                                        <div><a href="#">Sep. 20, 2018</a></div>
                                        <div><a href="#">Admin</a></div>
                                        <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                                    </div>
                                    <div className="desc pl-3">
                                        <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-4 ftco-animate">
                            <div className="blog-entry" data-aos-delay="200">
                                <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_3.jpg')})` }}>
                                </a>
                                <div className="text d-flex py-4">
                                    <div className="meta mb-3">
                                        <div><a href="#">Sep. 20, 2018</a></div>
                                        <div><a href="#">Admin</a></div>
                                        <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                                    </div>
                                    <div className="desc pl-3">
                                        <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </>
    );
    console.log("blog", blog);
};

export default Blog;